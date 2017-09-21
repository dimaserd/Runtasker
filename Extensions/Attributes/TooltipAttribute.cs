using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Attributes
{
    public class TooltipAttribute : CustomResourceAttribute
    {
        public TooltipAttribute(string text) : base(text)
        {

        }

        public TooltipAttribute(Type resourceType, string resourceName) : base(resourceType, resourceName)
        {

        }

        public string TooltipText
        {
            get
            {
                return Text;
            }
        }
    }
}
