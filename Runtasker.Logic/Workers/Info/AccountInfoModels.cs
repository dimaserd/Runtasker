using Extensions.String;
using Runtasker.LocaleBuilders.Models;
using Runtasker.Logic.Models;
using Runtasker.Resources.InfoModel;

namespace Runtasker.Logic.Workers.Info
{
    //a bunch of prepared InfoModels
    public class AccountInfoModels : UICultureSwitcher
    {
        public InfoModel ToConfirmEmail
        {
            get
            {
                return new InfoModel
                {
                    Title = string.Format(InfoModelRes.RegisterTitleFormat, InfoModelRes.Runtasker.WrapToStrong()),
                    Text = string.Format(InfoModelRes.RegisterTextFormat, InfoModelRes.RegisterTextToMark.WrapToStrong().WrapToEm(), InfoModelRes.Runtasker.WrapToStrong().WrapToEm()),
                };
            }
        }
    }
}
