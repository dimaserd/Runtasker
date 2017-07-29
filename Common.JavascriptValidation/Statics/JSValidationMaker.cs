using Newtonsoft.Json;
using Extensions.Reflection;
using Common.JavascriptValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public bool IsCheckBox { get; set; }
    }


    public class JSCode
    {
        /// <summary>
        /// Cам код проверочной функции
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Флаг указывающий на то надо ли код исполнять сразу
        /// </summary>
        public bool IsDefault { get; set; }
    }

    /// <summary>
    /// Свойство со списком 
    /// </summary>
    public class JsProperty
    {
        /// <summary>
        /// Имя свойства к которому принадлежат проверочные функции
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Проверочные джаваскрипт функции для свойства
        /// </summary>
        public List<string> CheckingScripts { get; set; }

        /// <summary>
        /// Исходные скрипты
        /// </summary>
        public List<string> DefaultScripts { get; set; }


        public string OnValueChangedHandler { get; set; }
    }


    

    public static class JSValidationMaker
    {
        

        public static string GetJSValidationObject(Type T)
        {
            List<JsProperty> result = GetJSValidationFuncsForProperties(T);

            return JsonConvert.SerializeObject(result);
        }


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

            foreach(string key in dict.Keys)
            {
                ScriptsForProperty propertyScripts = GetJSValidationFuncsForProperty(key, dict[key]);

                result.Add(new JsProperty
                {
                    PropertyName = key,
                    CheckingScripts = propertyScripts.CheckingScripts,
                    DefaultScripts = propertyScripts.DefaultScripts,
                    OnValueChangedHandler = GetJsOnChangeHandlerFromHalfScripts(key, propertyScripts.OnChangeHalfScripts, propertyScripts.IsCheckBox)
                });
            }

            return result;
        }


        public static ScriptsForProperty GetJSValidationFuncsForProperty(string propertyName, object[] attrs)
        {
            List<string> CheckingScripts = new List<string>();

            List<string> DefaultScripts = new List<string>();

            List<string> OnChangeHalfScripts = new List<string>();

            bool isCheckBox = false;

            foreach(object attr in attrs)
            {
                if(attr is JsNotValidateAttribute)
                {
                    CheckingScripts.Clear();
                    DefaultScripts.Clear();
                    OnChangeHalfScripts.Clear();

                    continue;
                }
                else if(attr is RequiredAttribute)
                {
                    CheckingScripts.Add(GetRequiredValidationFunc(propertyName, attr as RequiredAttribute));
                }
                else if (attr is MaxLengthAttribute)
                {
                    CheckingScripts.Add(GetMaxLengthValidationFunc(propertyName, attr as MaxLengthAttribute));
                }
                else if(attr is MinLengthAttribute)
                {
                    CheckingScripts.Add(GetMinLengthValidationFunc(propertyName, attr as MinLengthAttribute));
                }
                else if(attr is HideByDefaultAttribute)
                {
                    DefaultScripts.Add(GetHideByDefaultScript(propertyName, attr as HideByDefaultAttribute));
                }
                else if(attr is JsOnValueBaseAttribute)
                {
                    OnChangeHalfScripts.Add(GetJsOnValueHalfScript(propertyName, attr as JsOnValueBaseAttribute));
                }
                else if(attr is JsCheckBox)
                {
                    isCheckBox = true;
                }
            }

            return new ScriptsForProperty
            {
                CheckingScripts = CheckingScripts,
                DefaultScripts = DefaultScripts,
                OnChangeHalfScripts = OnChangeHalfScripts,
                IsCheckBox = isCheckBox
            };
        }

        /// <summary>
        /// Создает функцию обработчика 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="halfScripts"></param>
        /// <returns></returns>
        private static string GetJsOnChangeHandlerFromHalfScripts(string propertyName, List<string> halfScripts, bool isCheckBox = false)
        {
            if(halfScripts.Count == 0)
            {
                return "{}";
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("{")
            .Append($" console.log(\"Вызов функции изменения для {propertyName}\"); ")
            .Append($" var value = document.getElementById(\"{PropertyNameHelper.GetIdForInput(propertyName)}\").{((isCheckBox)? "checked.toString()" : "value")}; ")
            .Append(" console.log(value); ");

            halfScripts.ForEach(x => sb.Append(x));

            sb.Append("}");

            return sb.ToString();
        }


        #region Получение функции по атрибуту

        /// <summary>
        /// Часть джаваскрипт функции 
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private static string GetJsOnValueHalfScript(string propName, JsOnValueBaseAttribute attr)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append($" if(value === \"{attr.Value}\")  ")
            .Append("{")
            .Append(" console.log(\"Попадание в условие!\"); ")
            .Append($" {attr.Script} ")
            .Append("}");
            

            return sb.ToString();
        }

        private static string GetHideByDefaultScript(string propName, HideByDefaultAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" HideObject(\"{PropertyNameHelper.GetIdForForm(propName)}\"); ")
           
            .Append("}");

            return sb.ToString();
        }

        private static string GetRequiredValidationFunc(string propName, RequiredAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.length == 0) ")
            .Append(" { ")
            .Append($" WriteError('{propName}', '{attr.ErrorMessage}'); ")
            .Append(" return false; ")
            .Append(" } ")
            .Append(" else ")
            .Append(" { ")
            .Append($" HideError('{propName}'); ")
            .Append(" return true; ")
            .Append(" } ")
            .Append("}");

            return sb.ToString();
        }

        private static string GetMinLengthValidationFunc(string propName, MinLengthAttribute attr)
        {
            
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.length < {attr.Length}) ")
            .Append(" { ")
            .Append($" WriteError('{propName}', '{attr.ErrorMessage}'); ")
            .Append(" return false; ")
            .Append(" } ")
            .Append(" else ")
            .Append(" { ")
            .Append($" HideError('{propName}'); ")
            .Append(" return true; ")
            .Append(" } ")
            .Append("}");

            return sb.ToString();

        }

        private static string GetMaxLengthValidationFunc(string propName, MaxLengthAttribute attr)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append($" if (document.getElementById('{PropertyNameHelper.GetIdForInput(propName)}').value.length > {attr.Length}) ")
            .Append(" { ")
            .Append($" WriteError('{propName}', '{attr.ErrorMessage}'); ")
            .Append(" return false; ")
            .Append(" } ")
            .Append(" else ")
            .Append(" { ")
            .Append($" HideError('{propName}'); ")
            .Append(" return true; ")
            .Append(" } ")
            .Append("}");

            return sb.ToString();

        }
        #endregion
    }
}
