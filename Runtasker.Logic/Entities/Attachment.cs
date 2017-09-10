using Extensions.String;
using Runtasker.Logic.Workers.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace Runtasker.Logic.Entities
{
    #region Перечисления
    public enum AttachmentType
    {
        /// <summary>
        /// Обычное вложение ни к чему не относящееся
        /// </summary>
        Ordinary,

        /// <summary>
        /// Вложение для сообщения
        /// </summary>
        FromMessage,

        /// <summary>
        /// Вложенные файлы к заказу
        /// </summary>
        OrderFiles,
        
        /// <summary>
        /// Решение заказа
        /// </summary>
        OrderSolution,

        /// <summary>
        /// Файл загруженный администратором (например для редактирования Html страниц)
        /// </summary>
        AdminFile,
    }
    #endregion

    /// <summary>
    /// Сущность описывающая вложение (файл который может быть прикреплен к сообщению или к заказу)
    /// </summary>
    public class Attachment
    {
        public Attachment()
        {
            Id = Guid.NewGuid().ToString();
        }

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Id { get; set; }

        #region Для файлов
        /// <summary>
        /// [Устаревшее свойство больше не используется] Путь к физическому файлу в системе. (Используй с осторожностью,
        /// также он должен быть относительным).
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Имя файла. Используется для возвращения файла с нужным расширением из массива байтов.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Содержимое файла решения в байтах.
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// Майм тип файла. (используется для доставания содержимого файла обратно из массива байтов)
        /// </summary>
        public string FileMymeType { get; set; }
        #endregion

        public AttachmentType Type { get; set; }

        #region Свойства отношений

        [ForeignKey("Message")]
        public int? MessageId { get; set; }

        public virtual Message Message { get; set; }


        [ForeignKey("FromOrder")]
        public int? OrderId { get; set; }
        /// <summary>
        /// Файлы размещаемые заказчиком при создании заказа
        /// </summary>
        public virtual Order FromOrder { get; set; }

        
        #endregion


        #endregion
    }

    /// <summary>
    /// Класс инкапсулирующий некоторые сложные функции связанные с созданием вложений из физических файлов, директорий
    /// а также файлов принятых по Htpp.
    /// </summary>
    public static class AttachmentExtensions
    {
        /// <summary>
        /// Указывает является ли файл содержащийся внутри данного вложения архивом
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static bool InnerFileIsArchive(this Attachment attachment)
        {
            return attachment.FileName.EndsWith("zip");
        }

        #region Директории
        /// <summary>
        /// Возвращает путь к временной директории на диске
        /// </summary>
        public static string TempDirectory
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath("~/Files/Temp");
            }
        }

        /// <summary>
        /// Возвращает путь ко второй временной директории на диске
        /// </summary>
        public static string SecondTempDirectory
        {
            get
            {
                return System.Web.Hosting.HostingEnvironment.MapPath("~/Files/SecondTemp");
            }
        }

        /// <summary>
        /// Очищает временную директорию куда записываются файлы после архивации
        /// </summary>
        private static void ClearTempDirectories()
        {
            CheckDirectories();
            new DirectoryInfo(TempDirectory).Clear();
            new DirectoryInfo(SecondTempDirectory).Clear();
        }


        /// <summary>
        /// Проверяет наличие нужных директорий. Если их нет то создает их.
        /// </summary>
        private static void CheckDirectories()
        {
            string[] direcories = new string[]
            {
                TempDirectory, SecondTempDirectory
            };

            for(int i = 0; i < direcories.Length; i++)
            {
                if(!Directory.Exists(direcories[i]))
                {
                    Directory.CreateDirectory(direcories[i]);
                }
            }
        }
        #endregion

        #region Методы создания
        public static Attachment GetAttachmentFromFilesForOrder(IEnumerable<HttpPostedFileBase> files, int orderId)
        {
            Attachment result = GetAttachmentFromFiles(files);

            result.OrderId = orderId;

            return result;
        }

        public static Attachment GetAttachmentFromFile(HttpPostedFileBase file)
        {
            if(file == null)
            {
                return null;
            }

            ClearTempDirectories();

            string fileName = $"{TempDirectory}/{file.FileName.LeftJustFileName()}";

            file.SaveAs(fileName);

            return GetAttachmentFromFilePath(fileName);
        }

        public static Attachment GetAttachmentFromFiles(this IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null)
            {
                return null;
            }

            ClearTempDirectories();

            if(files.Count() > 1)
            {
                //записываю все файлы во вторую директорию
                foreach (HttpPostedFileBase file in files)
                {
                    if(file != null)
                    {
                        file.SaveAs($"{SecondTempDirectory}/{file.FileName.LeftJustFileName()}");
                    }
                }

                //создаю имя для файла
                string zipName = $"{Guid.NewGuid()}.zip";

                //задаю путь для архива в первой директории так как сами файлы находятся во второй
                string zipPath = $"{TempDirectory}/{zipName}";

                ZipFile.CreateFromDirectory(SecondTempDirectory, zipPath);

                
                return new Attachment
                {
                    Id = Guid.NewGuid().ToString(),
                    FileData = File.ReadAllBytes(zipPath),
                    FileName = zipName,
                    FileMymeType = MimeMapping.GetMimeMapping(zipPath),
                    FilePath = null,
                    MessageId = null,
                    OrderId = null
                };  

            }
            else
            {
                return GetAttachmentFromFile(files.First());
            }
            
        }

        public static Attachment GetAttachmentFromFilePath(string filePath)
        {
            return new Attachment
            {
                Id = Guid.NewGuid().ToString(),
                FileData = File.ReadAllBytes(filePath),
                FileMymeType = MimeMapping.GetMimeMapping(filePath),
                FileName = filePath.LeftJustFileName(),
                FilePath = null,
                MessageId = null,
                OrderId = null
            };

        }

        /// <summary>
        /// Создает сущность вложения из файлов находящихся в директории 
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static Attachment GetAttachmentFromDirectoryPath(string directoryPath)
        {
            CheckDirectories();

            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);

            List<FileInfo> files = dirInfo.EnumerateFiles().ToList();

            if(files.Count > 0)
            {
                if(files.Count == 1)
                {
                    return GetAttachmentFromFilePath(files.First().FullName);
                }
                else
                {
                    ClearTempDirectories();

                    string fileName = $"{Guid.NewGuid()}.zip";

                    string zipPath = $"{TempDirectory}/{fileName}";

                    ZipFile.CreateFromDirectory(directoryPath, zipPath);

                    return GetAttachmentFromFilePath(zipPath);
                }
            }
            else
            {
                throw new Exception($"{directoryPath} не содержит файлов!");
            }
            
            
        }

        /// <summary>
        /// Добавляет файлы в сущность описывающую вложение из переданного списка файлов
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static Attachment AddFilesToAttachment(this Attachment attachment, IEnumerable<HttpPostedFileBase> files)
        {
            ClearTempDirectories();

            //записываю все файлы во вторую директорию
            foreach(HttpPostedFileBase file in files)
            {
                if(file != null)
                {
                    file.SaveAs($"{SecondTempDirectory}/{file.FileName}");
                } 
            }

            if (attachment == null)
            {
                attachment = GetAttachmentFromDirectoryPath(SecondTempDirectory);

                

                return attachment;
            }
            else if (attachment.InnerFileIsArchive())
            {
                string zipPath = $"{TempDirectory}/{attachment.FileName}";

                //записываю файл на диск
                File.WriteAllBytes(zipPath, attachment.FileData);

                //вытаскивыю все файлы из предыдущего архива и засовываю во вторую директорию
                ZipFile.ExtractToDirectory(zipPath, SecondTempDirectory);

                //удаляю старый файл архива
                File.Delete(zipPath);

                //записываю вместо него новый содержащий еще и новые файлы
                ZipFile.CreateFromDirectory(SecondTempDirectory, zipPath);

                attachment.FileData = File.ReadAllBytes(zipPath);

                return attachment;
            }
            else
            {

                //записываю файл на диск туда же где и распакованы файлы из запроса
                File.WriteAllBytes($"{SecondTempDirectory}/{attachment.FileName}", attachment.FileData);

                //создаю имя для нового файла
                string zipName = $"{Guid.NewGuid()}.zip";

                //задаю путь для архива в первой директории так как сами файлы находятся во второй
                string zipPath = $"{TempDirectory}/{zipName}";

                ZipFile.CreateFromDirectory(SecondTempDirectory, zipPath);

                attachment.FileName = zipName;
                attachment.FileMymeType = MimeMapping.GetMimeMapping(zipPath);
                attachment.FileData = File.ReadAllBytes(zipPath);

                return attachment;
            }
            
        }
        #endregion

       

    }
}