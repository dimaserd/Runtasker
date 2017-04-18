using Extensions.HardCode;
using System;

namespace Runtasker.Logic.Attributes.OrderActionType
{
    public class ActionLinkAttribute : Attribute
    {
        public ActionLinkAttribute(Type resourceType, string actionTextResourceName, string linkFormat)
        {
            _linkFormat = linkFormat;
            ActionText = ResourceHelper.GetResourceLookup(resourceType, actionTextResourceName);
        }

        string _linkFormat; 

        public string ActionText { get; set; }

        public string GetLink()
        {
            return "";
        }
    }
}
