using Common.JavascriptValidation.Statics;
using Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JavascriptValidation.Attributes
{
    public class JsEmailAttribute : CustomResourceAttribute
    {
        public JsEmailAttribute() : base("Данное поле не является электронным адресом!")
        {

        }

        public static string GetCheckingFunction(string propName, JsEmailAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" var email = document.getElementById(\"{PropertyNameHelper.GetIdForInput(propName)}\"); ");
            sb.Append(" var re = /^ (([^<> ()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$/; ");
            sb.Append(" if !( re.test(email) ) ");
            sb.Append("{")
            .Append(JavaScriptHelper.WriteError(propName, attr.ErrorText))
            .Append(JavaScriptHelper.ReturnFalse)
            .Append("}")
            .Append(JavaScriptHelper.HideError(propName))
            .Append(JavaScriptHelper.ReturnTrue)
            .Append("}")
            .Append("}");

            return sb.ToString();
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
