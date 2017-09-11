namespace UI.Settings
{
    public static class AtroposWholeSettings
    {
        public static AtroposWholeSettingsModel GetDefaultSettings()
        {
            return new AtroposWholeSettingsModel
            {
                Footer = AtroposFooterSettings.GetSettings(),
                Header = AtroposHeaderSettings.GetSettings(),
                Main = AtroposSettings.GetSettings()
            };
        }

    }


    public class AtroposWholeSettingsModel
    {
        public AtroposFooterSettingsModel Footer { get; set; }

        public AtroposHeaderSettingsModel Header { get; set; }

        public AtroposSettingsModel Main { get; set; }
    }
}
