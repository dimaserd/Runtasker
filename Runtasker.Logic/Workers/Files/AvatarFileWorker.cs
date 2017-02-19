using System.IO;
using System.Linq;
using System.Web;

namespace Runtasker.Logic.Workers.Files
{
    public class AvatarFileWorker
    {
        #region Constants
        protected string[] extensions = { "jpg", "jpeg", "bmp" };

        protected string defaultImage
        {
            get
            {
                return $"{AvatarsDirectory}/default.jpg";
            }
        }

        #endregion

        #region Constructors

        public AvatarFileWorker(string userGuid, MyDbContext context)
        {
            Construct(userGuid, context);
        }

        void Construct(string userGuid = null, MyDbContext context = null)
        {
            UserGuid = userGuid;
            Context = context ?? new MyDbContext();
            RootDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~/Files");
            AvatarsDirectory = $"{RootDirectory}/Avatars";

            CheckForDirectories();
        }

        #endregion

        #region Preparation Methods
        private void CheckForDirectories()
        {
            if(!Directory.Exists(AvatarsDirectory))
            {
                Directory.CreateDirectory(AvatarsDirectory);
            }
        }
        #endregion

        #region Private Fields

        string UserGuid { get; set; }
        MyDbContext Context { get; set; }

        string RootDirectory { get; set; }
        string AvatarsDirectory { get; set; }
        #endregion

        #region Public Methods

        public void ChangeAvatarFile(HttpPostedFileBase image)
        {
            string ext = image.FileName.Split('.').Last();
            string filename = $"{AvatarsDirectory}/{UserGuid}.{ext}";
            image.SaveAs(filename);
        }

        public string GetAvatarPath()
        {
            string filename = "";
            string potentialFilename = "";
            foreach(string ext in extensions)
            {
                potentialFilename = $"{AvatarsDirectory}/{UserGuid}.{ext}";
                if (File.Exists(potentialFilename))
                {
                    filename = potentialFilename;
                    break;
                }
            }
            if(string.IsNullOrEmpty(filename))
            {
                return defaultImage;
            }
            return filename;
        }

        public string GetAvatarPathByGuid(string guid)
        {
            string potentialFilename = "";
            string filename = "";
            foreach (string ext in extensions)
            {
                potentialFilename = $"{AvatarsDirectory}/{guid}.{ext}";
                if (File.Exists(potentialFilename))
                {
                    filename = potentialFilename;
                    break;
                }
            }
            if (string.IsNullOrEmpty(filename))
            {
                return defaultImage;
            }
            return filename;
        }
        #endregion
    }
}
