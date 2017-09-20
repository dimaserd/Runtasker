using System.IO;

namespace Runtaker.LocaleBuiders.Workers
{
    public static class ResourceCopyWorker
    {
        public static void DoJob()
        {
            DirectoryInfo resDir = new DirectoryInfo(ResourceModelCreator.GetResourcesDir());

            string[] files = Directory.GetFiles(resDir.FullName, "*.*", System.IO.SearchOption.AllDirectories);

            

            foreach(string file in files)
            {

                if(file.Contains(".Copy"))
                {
                    File.Delete(file);
                }
                else if(file.Contains(".en-GB.en-GB"))
                {
                    File.Delete(file);
                }

                else if(file.Contains(".Copy.en-GB"))
                {
                    File.Delete(file);
                }

                
                else if (file.Contains("en-GB"))
                {
                    
                    File.Delete(file);
                  
                }
                
            }
        }
    }
}
