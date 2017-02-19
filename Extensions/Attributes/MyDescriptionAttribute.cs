using Extensions.HardCode;
using System;


namespace Extensions.Attributes
{
    public class MyDescriptionAttribute : Attribute
    {

        public MyDescriptionAttribute(Type resourceType, string resourceName)
        {
            Description = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }

        public string Description { get; set; }
    }
}
