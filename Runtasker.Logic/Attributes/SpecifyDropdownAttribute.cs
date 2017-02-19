using Extensions.HardCode;
using System;

namespace Runtasker.Logic.Attributes
{
    public class SpecifyDropdownAttribute : Attribute
    {

        public SpecifyDropdownAttribute(Type resourceType, string resourceName)
        {
            DropdownParams = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }

        public SpecifyDropdownAttribute(Type resourceType, string dropdownName, string labelName)
        {
            DropdownParams = ResourceHelper.GetResourceLookup(resourceType, dropdownName);
            Label = ResourceHelper.GetResourceLookup(resourceType, labelName);
        }

        public string DropdownParams { get; set; }

        public string Label { get; set; }
    }
}
