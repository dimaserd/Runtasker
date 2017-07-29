using System;

namespace Extensions.Attributes
{
    public class PlaceholderAttribute : CustomResourceAttribute
    {
        #region Конструкторы

        public PlaceholderAttribute(string text) : base(text)
        {

        }

        public PlaceholderAttribute(Type resourceType, string resourceName) : base(resourceType, resourceName)
        {

        }

        #endregion

    }
}
