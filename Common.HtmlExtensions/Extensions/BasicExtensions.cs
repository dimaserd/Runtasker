using Extensions.Dictionary;
using oksoft.Common.HtmlExtensions.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace oksoft.Common.HtmlExtensions.Extensions
{
    public static class BasicExtensions
    {
        public static MvcHtmlString ExtendedDropdownList(this HtmlHelper html, string propName, IEnumerable<ExtendedSelectListItem> selectList, Dictionary<string, object> htmlAttributes = null)
        {

            StringBuilder sb = new StringBuilder();

            string propString = (htmlAttributes != null) ? htmlAttributes.GetPropertiesString() : string.Empty;

            List<SelectListGroup> groupNames = selectList.Select(x => x.Group).Distinct().ToList();
            
            sb.Append($"<select name=\"{propName}\" {propString}>");

            if(groupNames.Count != 0)
            {
                foreach(SelectListGroup optGroup in groupNames)
                {
                    sb.Append($"<optgroup label=\"{optGroup.Name}\">");
                    foreach(ExtendedSelectListItem it in selectList.Where(x => x.Group == optGroup))
                    {
                        sb.Append(it.ToString());
                    }
                    sb.Append("</optgroup>");
                }
            }
            else
            {
                foreach (ExtendedSelectListItem it in selectList)
                {
                    sb.Append(it.ToString());
                }
            }

            sb.Append("</select>");

            

            return MvcHtmlString.Create(sb.ToString());
        }

    }
}
