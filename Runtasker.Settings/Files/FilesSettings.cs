using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runtasker.Settings.Files
{
    public static class FilesSettings
    {
        #region Константы

        static string[] ImageExtensions = new string[]
        {
            "jpg", "jpeg", "tiff", "png", "bmp"
        };

        static string[] DocumentExtensions = new string[]
        {
            "docx", "doc", "xls", "xlsx", "txt",  "ppt", "pptx", "pdf"
        };

        static string[] MusicExtensions = new string[]
        {
            "mp3", "flac", "ape", "ogg", "waw", "ac3", "wma", "m4a",
        };

        static string[] ArchiveExtensions = new string[]
        {
            "rar", "zip", "7z", "tar", "gzip", "gz", "jar"
        };
        #endregion

        #region Публичные Методы
        public static bool IsThatFileImage(HttpPostedFileBase file)
        {
            
            string ext = GetExtension(file);

            return ImageExtensions.Any(x => x == ext);
        }

        public static bool IsThatGoodFile(HttpPostedFileBase file)
        {
            string ext = GetExtension(file);

            List<string> allExtensions = new List<string>();

            allExtensions.AddRange(ImageExtensions);
            allExtensions.AddRange(DocumentExtensions);
            allExtensions.AddRange(MusicExtensions);
            allExtensions.AddRange(ArchiveExtensions);

            return allExtensions.Any(x => x == ext);
        }
        #endregion

        #region Вспомогательные методы
        public static string GetExtension(HttpPostedFileBase file)
        {
            string fileName = file.FileName;

            string ext = fileName.Split(separator: new string[] { "." }, options: System.StringSplitOptions.RemoveEmptyEntries).Last();

            return ext;
        }
        #endregion
    }
}
