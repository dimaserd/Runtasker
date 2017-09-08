namespace Runtasker.Settings
{
    /// <summary>
    /// Содержит настройки о скидках бонусах и так далее
    /// </summary>
    public static class UISettings
    {
        #region Список бонусов
        /// <summary>
        /// Бонус при регистрации 
        /// </summary>
        public const int RegistrationBonus = 300;
        #endregion

        #region Стартовые цены
        public const int OrdinaryFromPrice = 50;

        public const int EssayFromPrice = 200;

        public const int CourseWorkFromPrice = 500;

        public const int OnlineHelpFromPrice = 200;
        #endregion

        #region Сроки
        public static int DaysForOrdinaryTask = 3;

        public static int DaysForEssayTask = 7;

        public static int DaysForCourseWorkTask = 30;
        #endregion

        public static string VkGroupLink = "https://vk.com/runtasker";

        public static int CompanyStartYear = 2014;
    }
}
