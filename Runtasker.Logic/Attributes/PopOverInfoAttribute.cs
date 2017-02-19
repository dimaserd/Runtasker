using Extensions.HardCode;
using System;

namespace Runtasker.Logic.Attributes
{
    
    public class PopoverInfoAttribute : Attribute
    {

        public PopoverInfoAttribute(Type resourceType, string resourceName)
        {
            Info = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }

        public string Info { get; set; }
    }

    
}
