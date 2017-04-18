using Extensions.String;
using Runtasker.Logic.Enumerations.InfoModels;
using Runtasker.Logic.Models;
using Runtasker.Resources.InfoModel;

namespace Runtasker.Logic.Workers.Info
{

    public static class AccountInfoModels
    {
        public static InfoModel GetInfoModel(InfoModelType? type)
        {
            switch(type)
            {
                case InfoModelType.ToConfirmEmail:
                    return new InfoModel
                    {
                        Title = string.Format(InfoModelRes.RegisterTitleFormat, InfoModelRes.Runtasker.WrapToStrong()),
                        Text = string.Format(InfoModelRes.RegisterTextFormat, InfoModelRes.RegisterTextToMark.WrapToStrong().WrapToEm(), InfoModelRes.Runtasker.WrapToStrong().WrapToEm()),
                    };

                default: return null;
            }
        }
        
    }
}
