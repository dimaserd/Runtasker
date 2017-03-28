﻿namespace Runtasker.Settings
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
        public static int RegistrationBonus = 300;
        #endregion

        #region Стартовые цены
        public static int OrdinaryFromPrice = 50;

        public static int EssayFromPrice = 200;

        public static int CourseWorkFromPrice = 500;
        #endregion

        #region Сроки
        public static int DaysForOrdinaryTask = 3;

        public static int DaysForEssayTask = 7;

        public static int DaysForCourseWorkTask = 30;
        #endregion
    }
}
