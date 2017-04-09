namespace Runtasker.Settings
{
    public static class DevSettings
    {
        /// <summary>
        /// Массив из ролей пользователей, которые могут быть в приложении
        /// </summary>
        public static string[] RolesInApp = new string[] { "Admin", "Customer", "Performer", "VkPerformer" };

        public static string TestCustomerEmail = "dimaserd96@yandex.ru";

        public static string AdminEmail = AdminSettings.AdminEmail;

        public static string TestPassword = "testpass";

        public static string AdminVkDomain = "dimaserd";
    }
}
