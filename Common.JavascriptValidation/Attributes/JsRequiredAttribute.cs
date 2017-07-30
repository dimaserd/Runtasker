using Extensions.Attributes;
using System;

namespace Common.JavascriptValidation.Attributes
{
    public class JsRequiredAttribute : CustomResourceAttribute
    {
        public JsRequiredAttribute(string ErrorText) : base(ErrorText)
        {

        }

        public JsRequiredAttribute(string resourceName, Type resourceType) : base(resourceType, resourceName)
        {

        }

        public string ErrorText
        {
            get
            {
                return Text;
            }
        }
    }
}
