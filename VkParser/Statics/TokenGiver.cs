using System.IO;
using System.Web.Hosting;

namespace VkParse.Statics
{
    public class TokenGiver
    {
        #region Fields
        private string _token;
        #endregion

        #region Protected Properties
        public string Token
        {
            get
            {
                if(_token == null)
                {
                    SetToken();
                }

                return _token;
            }
        }
        #endregion

        #region Help Methods
        private void SetToken()
        {
            string filesDir = HostingEnvironment.MapPath("~/Files");

            string tokenFilePath = $"{filesDir}/vk_token.txt";

            if (File.Exists(tokenFilePath))
            {
                _token =  File.ReadAllText(tokenFilePath);
            }
        }
        #endregion
    }
}
