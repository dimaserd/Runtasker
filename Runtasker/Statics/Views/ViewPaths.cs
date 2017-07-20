namespace Runtasker.Statics.Views
{
    /// <summary>
    /// Содержит статические пути к мастер представлениям.
    /// </summary>
    public static class ViewPaths
    {
        private const string LayoutsDirectory = "~/Views/Layouts";

        public static string ModalLayoutPath
        {
            get
            {
                return $"{LayoutsDirectory}/_ModalLayout.cshtml";
            }
        }

        public static string NewLayoutPath
        {
            get
            {
                return $"{LayoutsDirectory}/_NewLayout.cshtml";
            }
        }

        public static string NewAdminLayoutPath
        {
            get
            {
                return $"{LayoutsDirectory}/_NewAdminLayout.cshtml";
            }
        }

        public static string EmptyLayoutPath
        {
            get
            {
                return $"{LayoutsDirectory}/_EmptyLayout.cshtml";
            }
        }
    }
}