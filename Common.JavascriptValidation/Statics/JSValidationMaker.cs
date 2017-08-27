using Newtonsoft.Json;
using Extensions.Reflection;
using Common.JavascriptValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Common.JavascriptValidation.Enumerations;
using Common.JavascriptValidation.Entities;

namespace Common.JavascriptValidation.Statics
{
    public class ScriptsForProperty
    {
        public List<string> CheckingScripts { get; set; }

        public List<string> DefaultScripts { get; set; }

        /// <summary>
        /// Части скрипта которые должны быть собраны чтобы описать какоое
        /// </summary>
        public List<string> OnChangeHalfScripts { get; set; }


        public JsHtmlDataType HtmlDataType { get; set; }
    }


    public static class JSValidationMaker
    {

        #region Методы для хелпера
        public static string GetJSValidationObject(Type T)
        {
            List<JsProperty> result = GetJSValidationFuncsForProperties(T);

            return JsonConvert.SerializeObject(result);
        }

        public static string GetJSValidationObject(JsModelWithLabels model)
        {
            List<JsProperty> result = GetJSValidationFuncsForProperties(model);

            return JsonConvert.SerializeObject(result);
        }
        #endregion

        #region Вспомогательные методы для хелпера
        /// <summary>
        /// Возвращает словарь имя_свойства -- список из текстов js функций проверок
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public static List<JsProperty> GetJSValidationFuncsForProperties(Type T)
        {
            Dictionary<string, object[]> dict = ReflectionExtensionMethods
                .GetPropertyAttributesDictionary(T);

            List<JsProperty> result = new List<JsProperty>();

            foreach (string key in dict.Keys)
            {
                ScriptsForProperty propertyScripts = GetJSValidationFuncsForProperty(key, dict[key]);

                result.Add(new JsProperty
                {
                    PropertyName = key,
                    CheckingScripts = propertyScripts.CheckingScripts,
                    DefaultScripts = propertyScripts.DefaultScripts,
                    OnValueChangedHandler = GetJsOnChangeHandlerFromHalfScripts(key, propertyScripts.OnChangeHalfScripts, propertyScripts.HtmlDataType),
                    HtmlDataType = propertyScripts.HtmlDataType
                });
            }

            return result;
        }

        /// <summary>
        /// Возвращает словарь имя_свойства -- список из текстов js функций проверок
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public static List<JsProperty> GetJSValidationFuncsForProperties(JsModelWithLabels model)
        {
            List<JsProperty> result = new List<JsProperty>();

            foreach (JsPropertyWithLabel jsProp in model)
            {
                result.Add(new JsProperty
                {
                    PropertyName = jsProp.PropertyName,
                    CheckingScripts = jsProp.CheckingScripts,
                    DefaultScripts = jsProp.DefaultScripts,
                    OnValueChangedHandler = jsProp.OnValueChangedHandler
                });
            }

            return result;
        }

        #endregion


        public static JsProperty GetJsProperty(string propertyName, JsHtmlDataType dataType, object[] attrs)
        {
            ScriptsForProperty scriptsModel = GetJSValidationFuncsForProperty(propertyName, attrs);

            return new JsProperty
            {
                PropertyName = propertyName,
                CheckingScripts = scriptsModel.CheckingScripts,
                DefaultScripts = scriptsModel.DefaultScripts,
                OnValueChangedHandler = GetJsOnChangeHandlerFromHalfScripts(propertyName, scriptsModel.OnChangeHalfScripts, scriptsModel.HtmlDataType)
            };
        }


        public static ScriptsForProperty GetJSValidationFuncsForProperty(string propertyName, object[] attrs)
        {
            List<string> CheckingScripts = new List<string>();

            List<string> DefaultScripts = new List<string>();

            List<string> OnChangeHalfScripts = new List<string>();

            JsHtmlDataType DataType = JsHtmlDataType.Text;


            foreach (object attr in attrs)
            {
                if (attr is JsNotValidateAttribute)
                {
                    CheckingScripts.Clear();
                    DefaultScripts.Clear();
                    OnChangeHalfScripts.Clear();
                    DataType = JsHtmlDataType.Text;


                    continue;
                }
                else if (attr is JsHtmlDataTypeAttribute)
                {
                    JsHtmlDataTypeAttribute dataTypeAttr = attr as JsHtmlDataTypeAttribute;

                    DataType = dataTypeAttr.DataType;
                }
                else if (attr is JsRequiredAttribute)
                {
                    CheckingScripts.Add(JsRequiredAttribute.GetRequiredValidationFunc(propertyName, attr as JsRequiredAttribute));
                }
                else if (attr is JsCompareAttribute)
                {
                    CheckingScripts.Add(JsCompareAttribute.GetJsCompareCheckingScript(propertyName, attr as JsCompareAttribute));
                }
                else if (attr is JsMaxLengthAttribute)
                {
                    CheckingScripts.Add(JsMaxLengthAttribute.GetJsCheckFunction(propertyName, attr as JsMaxLengthAttribute));
                }
                else if (attr is JsMinLengthAttribute)
                {
                    CheckingScripts.Add(JsMinLengthAttribute.GetJsCheckFunction(propertyName, attr as JsMinLengthAttribute));
                }
                else if (attr is JsHideByDefaultAttribute)
                {
                    DefaultScripts.Add(JsHideByDefaultAttribute.GetHideByDefaultScript(propertyName, attr as JsHideByDefaultAttribute));
                }
                else if (attr is JsOnValueWithElseAttribute)
                {
                    OnChangeHalfScripts.Add(JsOnValueWithElseAttribute.GetJsOnValueWithElseHalfScript(propertyName, attr as JsOnValueWithElseAttribute));
                }
                else if (attr is JsOnValueBaseAttribute)
                {
                    OnChangeHalfScripts.Add(JsOnValueBaseAttribute.GetJsOnValueHalfScript(propertyName, attr as JsOnValueBaseAttribute));
                }
                else if (attr is JsDefaultValueAttribute)
                {
                    DefaultScripts.Add(JsDefaultValueAttribute.GetJsDefaultValueScript(propertyName, attr as JsDefaultValueAttribute));
                }

            }

            return new ScriptsForProperty
            {
                CheckingScripts = CheckingScripts,
                DefaultScripts = DefaultScripts,
                OnChangeHalfScripts = OnChangeHalfScripts,
                //IsCheckBox = isCheckBox
                HtmlDataType = DataType
            };
        }
        /// <summary>
        /// Создает функцию обработчика 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="halfScripts"></param>
        /// <returns></returns>
        private static string GetJsOnChangeHandlerFromHalfScripts(string propertyName, List<string> halfScripts, JsHtmlDataType dataType = JsHtmlDataType.Text)
        {
            if (halfScripts.Count == 0)
            {
                return "{}";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{")
            .Append($" console.log(\"Вызов функции изменения для {propertyName}\"); ")
            .Append($" var value = document.getElementById(\"{PropertyNameHelper.GetIdForInput(propertyName)}\").{((dataType == JsHtmlDataType.CheckBox) ? "checked.toString()" : "value")}; ")
            .Append(" console.log(value); ");

            halfScripts.ForEach(x => sb.Append(x));

            sb.Append("}");

            return sb.ToString();
        }

    }
}
