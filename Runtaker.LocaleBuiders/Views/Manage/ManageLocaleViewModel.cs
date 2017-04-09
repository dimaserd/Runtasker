using Runtasker.LocaleBuilders.Models;
using Runtasker.Resources.Views.Manage.ChangePassword;
using Runtasker.Resources.Views.Manage.SetPassword;

namespace Runtasker.LocaleBuilders.Views.Manage
{
    public class ManageLocaleViewModelBuilder : UICultureSwitcher
    {
        public LocaleViewModel SetPasswordView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", SetPassRes.Title);
            result.Add("Header", SetPassRes.Header);
            result.Add("InfoText", SetPassRes.Info);
            result.Add("LittleHeader", SetPassRes.Title);
            result.Add("BtnText", SetPassRes.SetPass);

            return result;
        }

        public LocaleViewModel ChangePasswordView()
        {
            LocaleViewModel result = new LocaleViewModel();

            result.Add("Title", ChangePassRes.Title);
            result.Add("Header", ChangePassRes.Header);
            result.Add("ActionDesc", ChangePassRes.ActionDesc);
            result.Add("BtnText", ChangePassRes.ChangePass);

            return result;
        }
    }
}
