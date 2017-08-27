namespace Common.JavascriptValidation.Statics
{
    public static class JavaScriptHelper
    {
        /// <summary>
        /// Консоль лог
        /// </summary>
        /// <returns></returns>
        public static string ConsoleLog(string value)
        {
            return $"console.log(\"{value}\")";
        }

        public static string ReturnFalse
        {
            get
            {
                return " return false; ";
            }
        }

        public static string ReturnTrue
        {
            get
            {
                return " return true; ";
            }
        }

        public static string WriteError(string propName, string errorText)
        {
            return $" WriteError('{propName}', '{errorText}'); ";
        }

        public static string HideError(string propName)
        {
            return $" HideError('{propName}'); ";
        }
    }
}
