using Extensions.HardCode;
using System;

namespace Runtasker.Logic.Attributes
{
    public class ErrorTextAttribute : Attribute
    {
        public ErrorTextAttribute(Type resourceType, string resourceName)
        {
            Text = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }

        public string Text { get; set; }
    }
}
