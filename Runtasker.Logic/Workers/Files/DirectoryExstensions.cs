using System.IO;

namespace Runtasker.Logic.Workers.Files
{
    public static class DirectoryExtensions
    {
        public static void Clear(this DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}
