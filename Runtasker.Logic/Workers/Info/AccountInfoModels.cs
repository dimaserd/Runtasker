using Extensions.String;
using Runtasker.LocaleBuilders.Enumerations;
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
                switch(UILang)
                {
                    case Lang.Russian:
                        return new InfoModel
                        {
                            Title = InfoModelRes.RegisterTitle1 + 
                            $" {InfoModelRes.Runtasker.WrapToStrong()}",
                            Text = $"{InfoModelRes.RegisterText1} " + 
                            $"{InfoModelRes.RegisterSurpriseText1.WrapToStrong().WrapToEm()} " +
                            $"{InfoModelRes.RegisterText2} {InfoModelRes.Runtasker.WrapToStrong().WrapToEm()}!"
                        };

                    default:
                        return new InfoModel
                        {
                            Title = InfoModelRes.RegisterTitle1 + 
                            $" {InfoModelRes.Runtasker.WrapToStrong()} " +
                            InfoModelRes.RegisterTitle2,
                            Text = $"{InfoModelRes.RegisterText1} " +
                            $"{InfoModelRes.RegisterSurpriseText1.WrapToStrong().WrapToEm()} " +
                            $"{InfoModelRes.RegisterText2} {InfoModelRes.Runtasker.WrapToStrong().WrapToEm()}!"
                        };
                }
                
            }
        }
    }
}
