using Runtasker.Logic.Workers.Files;
using System.Data;
using System.Web;

namespace Runtasker.Logic.Workers
{
    public class AvatarWorker
    {
        #region Constants
        protected string[] ImageMimeTypes = { "image/jpeg", "image/jpeg", "image/bmp" };
        #endregion

        #region Constructors

        public AvatarWorker(string userGuid)
        {
            Construct(userGuid, null);
        }

        public AvatarWorker(string userGuid, MyDbContext context)
        {
            Construct(userGuid, context);
        }

        protected void Construct(string userGuid = null, MyDbContext context = null)
        {
            UserGuid = userGuid;
            Context = context ?? new MyDbContext();
            AvatarFiler = new AvatarFileWorker(UserGuid, Context);
        }

        #endregion

        #region Private Fields
        string UserGuid { get; set; }
        MyDbContext Context { get; set; }
        AvatarFileWorker AvatarFiler { get; set; }
        #endregion

        #region Public Methods
        

        public void ChangeAvatar(HttpPostedFileBase image)
        {
            if(image != null && isRightMimeType(image))
            {
                AvatarFiler.ChangeAvatarFile(image);
            }
        }

        public string GetAvatarPath()
        {
            return AvatarFiler.GetAvatarPath();
        }

        public string GetAvatarPathByGuid(string guid)
        {
            return AvatarFiler.GetAvatarPathByGuid(guid);
        }
        #endregion

        #region Private Helping Methods
        bool isRightMimeType(HttpPostedFileBase file)
        {
            bool result = false;
            foreach(string mimeType in ImageMimeTypes)
            {
                if(file.ContentType == mimeType)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion
    }
}
