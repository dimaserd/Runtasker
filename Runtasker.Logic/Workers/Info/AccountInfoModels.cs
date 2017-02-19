

using Runtasker.Logic.Models;
using Runtasker.Resources.InfoModel;

namespace Runtasker.Logic.Workers.Info
{
    //a bunch of prepared InfoModels
    public class AccountInfoModels
    {
        public InfoModel ToConfirmEmail
        {
            get
            {
                return new InfoModel
                {
                    Title = InfoModelRes.RegisterTitle,
                    Text = InfoModelRes.RegisterText
                };
            }
        }
    }
}
