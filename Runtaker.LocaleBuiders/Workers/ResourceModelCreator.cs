using Runtaker.LocaleBuiders.Entities;
using Runtaker.LocaleBuiders.Interfaces;
using Runtaker.LocaleBuiders.Settings;
using Runtasker.LocaleBuilders.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Runtaker.LocaleBuiders.Workers
{
    public static class ResourceModelCreator
    {
        private static string GetResourcesDir()
        {
            if(LocaleBuilderSettings.UseCustomPath)
            {
                return LocaleBuilderSettings.CustomPath;
            }

            //C:\Users\dmitryserdyukov\Source\Repos\Runtasker\Console\bin\Debug
            DirectoryInfo projDir = new DirectoryInfo(Directory.GetCurrentDirectory());

            DirectoryInfo runtaskerDir = projDir.Parent.Parent.Parent;


            DirectoryInfo[] runtaskerDirs = runtaskerDir.GetDirectories();

            return runtaskerDirs.ToList().FirstOrDefault(x => x.Name == "Runtasker.Resources").FullName;
        }

        public static IEnumerable<ResourceFileModel> GetModels()
        {
            List<Lang> langs = Enum.GetValues(typeof(Lang)).Cast<Lang>().ToList();

            return langs.SelectMany(x => GetModelsByLang(x));
        }


        public static IEnumerable<ResourceFileModel> GetModelsByLang(Lang lang = Lang.English)
        {
            DirectoryInfo resDir = new DirectoryInfo(GetResourcesDir());

            DirectoryInfo[] resDirs = resDir.GetDirectories();


            List<FileInfo> result = new List<FileInfo>();

            foreach(DirectoryInfo dir in resDirs)
            {
                result.AddRange(GetResxFilesDromDirectory(dir, lang).ToList());
            }
            return GetResourceFileModels(result, lang).ToList();
        }

        public static void UpdateResourceFile(Hashtable data, string path)
        {
            Hashtable resourceEntries = new Hashtable();

            //Get existing resources
            ResourceReader reader = new ResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                }
                reader.Close();
            }

            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {

                    String value = data[key].ToString();
                    if (value == null) value = "";

                    resourceEntries.Add(key, value);
                }
            }

            //Write the combined resource file
            ResourceWriter resourceWriter = new ResourceWriter(path);

            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();

        }

        private static List<ResourceString> GetStringsFromResxFile(ResourceFileModel fileModel)
        {
            string resxPath = $"{GetResourcesDir()}{fileModel.ResourcePath}";

            return GetStringsFromResxFilePath(resxPath);
        }

        private static List<ResourceString> GetStringsFromResxFilePath(string resxPath)
        {
            
            if (!File.Exists(resxPath))
            {
                return null;
            }


            ResXResourceReader reader = new ResXResourceReader(resxPath);

            List<ResourceString> result = new List<ResourceString>();

            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    result.Add(new ResourceString
                    {
                        Id = Guid.NewGuid().ToString(),
                        LastEditedDate = DateTime.Now,
                        ResourceKey = d.Key.ToString(),
                        ResourceValue = (d.Value == null) ? "" : d.Value.ToString(),
                    });
                }
                reader.Close();
            }

            return result;
        }

        #region Вспомогательные методы
        private static IEnumerable<ResourceFileModel> GetResourceFileModels(IEnumerable<FileInfo> files, Lang lang)
        {
            string sep = "Runtasker.Resources";

            return files.ToList().Select(x => new ResourceFileModel
            {
                Id = Guid.NewGuid().ToString(),
                CreateDate = x.CreationTime,
                ResourcePath = x.FullName.Split(separator: new string[] { sep }, options: StringSplitOptions.None).Last(),
                LangCode = lang,
                ResourceStrings = GetStringsFromResxFilePath(x.FullName)
            });

        }

        private static IEnumerable<FileInfo> GetResxFilesDromDirectory(DirectoryInfo dir, Lang lang = Lang.English)
        {

            string ending = lang.GetFileEnding();

            if(lang == Lang.English)
            {
                string rusEnding = Lang.Russian.GetFileEnding();

                string chEnding = Lang.Chinese.GetFileEnding();

                return Directory.GetFiles(dir.FullName, "*.*", System.IO.SearchOption.AllDirectories)
                .Where(file => file.EndsWith(ending) && !file.EndsWith(rusEnding) && !file.EndsWith(chEnding))
                .Select(x => new FileInfo(x));
            }
            else
            {
                return Directory.GetFiles(dir.FullName, "*.*", System.IO.SearchOption.AllDirectories)
                .Where(file => file.EndsWith(ending))
                .Select(x => new FileInfo(x));
            }
            
        }
        #endregion
    }
}
