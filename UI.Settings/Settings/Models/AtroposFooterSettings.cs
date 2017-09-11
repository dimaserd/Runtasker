namespace UI.Settings
{
    public static class AtroposFooterSettings
    {
        public static bool EmailSubscriptionEnabled = true;

        public static AtroposFooterSettingsModel GetSettings()
        {
            return new AtroposFooterSettingsModel
            {
                EmailSubscriptionEnabled = EmailSubscriptionEnabled
            };
        }

        public static void ChangeSettings(AtroposFooterSettingsModel model)
        {
            EmailSubscriptionEnabled = model.EmailSubscriptionEnabled;
        }
    }

    public class AtroposFooterSettingsModel
    {
        public bool EmailSubscriptionEnabled { get; set; }

        
    }
}