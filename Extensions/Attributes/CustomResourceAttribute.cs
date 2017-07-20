using Extensions.HardCode;
using System;


namespace Extensions.Attributes
{
    /// <summary>
    /// Аттрибут содержищий свойство текст и умеющий получать значение свойства из файлов ресурсов
    /// </summary>
    public class CustomResourceAttribute : Attribute
    {
        #region Конструкторы
        public CustomResourceAttribute(string text)
        {
            Text = text;
        }

        public CustomResourceAttribute(Type resourceType, string resourceName)
        {
            Text = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }
        #endregion

        public string Text { get; set; }
    }
}
