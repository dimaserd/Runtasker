using System.IO;
using System.Web.Hosting;

namespace VkParse.Statics
{
    public class TokenGiver
    {
        #region Fields
        private string _token;

        string _appToken;
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

        public string AppToken
        {
            get
            {
                if (_appToken == null)
                {
                    SetAppToken();
                }

                return _appToken;
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

        private void SetAppToken()
        {
            string filesDir = HostingEnvironment.MapPath("~/Files");

            string tokenFilePath = $"{filesDir}/vkApp_token.txt";

            if (File.Exists(tokenFilePath))
            {
                _appToken = File.ReadAllText(tokenFilePath);
            }
        }
        #endregion
    }
}
