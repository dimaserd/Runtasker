namespace Common.JavascriptValidation.Statics
{
    /// <summary>
    /// Класс посредник который соединяет формы 
    /// и джаваскриптовские обработчики
    /// </summary>
    public static class PropertyNameHelper
    {
        /// <summary>
        /// Постфикс который добавляется к имени свойства для текста с ошибкой
        /// </summary>
        public const string ErrorTextAddition = "ErrorText";

        /// <summary>
        /// Постфикс который добавляется к имени свойства для формы ввода
        /// </summary>
        public const string FormAddition = "Form";

        /// <summary>
        /// Постфикс который добавляется к имени свойства для самого ввода
        /// </summary>
        public const string InputAddition = "Input";

        /// <summary>
        /// Возвращает строку значения Id для редактора свойства по имени свойства модели
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetIdForInput(string propertyName)
        {
            return $"{propertyName}{InputAddition}";
        }


        /// <summary>
        /// Возвращает строку значения Id для формы свойства по имени свойства модели
        /// (на форму джаваскриптом ставится класс ошибки - который подсвечивает это красным)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetIdForForm(string propertyName)
        {
            return $"{propertyName}{FormAddition}";
        }

        /// <summary>
        /// Возвращает строку значения Id для формы свойства по имени свойства модели
        /// (джаваскриптом меняется класс видимости элемента и вставляется текст ошибки - и исчезает
        /// когда ошибка ввода отсутствует)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetIdForTextError(string propertyName)
        {
            return $"{propertyName}{ErrorTextAddition}";
        }

    }
}
