using Runtasker.Settings.Enumerations;

namespace Runtasker.Settings
{
    public static class Settings
    {
        /// <summary>
        /// Local переключает приложение на локальную базу,
        /// Production на рабочую базу данных
        /// </summary>
        public static ConnectionType Connection = ConnectionType.Production;

        public static ApplicationSettingType AppSetting = ApplicationSettingType.Production;
    }
}
