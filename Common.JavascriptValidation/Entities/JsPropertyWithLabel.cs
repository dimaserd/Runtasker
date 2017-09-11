using Common.JavascriptValidation.Enumerations;
using Common.JavascriptValidation.Statics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.JavascriptValidation.Entities
{
    //Сущность описывающая своство в модели
    public class JsPropertyWithLabel : JsProperty
    {
        #region Конструкторы
        public JsPropertyWithLabel()
        {

        }

        public JsPropertyWithLabel(JsProperty jsProp, string labelText)
        {
            Construct(jsProp, labelText);
        }

        public JsPropertyWithLabel(string labelText, string propName)
        {
            LabelText = labelText;
            PropertyName = propName;
            CheckingScripts = new List<string>();
            DefaultScripts = new List<string>();
        }

        public JsPropertyWithLabel(string labelText, string propName, JsHtmlDataType dataType)
        {
            LabelText = labelText;
            PropertyName = propName;
            CheckingScripts = new List<string>();
            DefaultScripts = new List<string>();
            HtmlDataType = dataType;
        }

        public JsPropertyWithLabel(string labelText, string propName, JsHtmlDataType dataType, params Attribute[] jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes);

            Construct(jsProp, labelText);
        }



        public JsPropertyWithLabel(string labelText, string propName, string placeholderValue, JsHtmlDataType dataType, params Attribute[] jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes);

            Construct(jsProp, labelText, placeholderValue);
        }

        public JsPropertyWithLabel(string labelText, string propName, JsHtmlDataType dataType, IEnumerable<Attribute> jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes.ToArray());

            Construct(jsProp, labelText);
        }

        public JsPropertyWithLabel(string labelText, string propName, string placeholderValue, JsHtmlDataType dataType, IEnumerable<Attribute> jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes.ToArray());

            Construct(jsProp, labelText, placeholderValue);

            HtmlDataType = dataType;
        }


        public JsPropertyWithLabel(string labelText, string propName, JsHtmlDataType dataType, JsInputDataType inputType, IEnumerable<Attribute> jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes.ToArray());

            InputType = inputType;

            Construct(jsProp, labelText);
        }

        public JsPropertyWithLabel(string labelText, string propName, string placeholderValue, JsHtmlDataType dataType, JsInputDataType inputType, IEnumerable<Attribute> jsAttributes)
        {
            JsProperty jsProp = JSValidationMaker.GetJsProperty(propName, dataType, jsAttributes.ToArray());

            InputType = inputType;

            Construct(jsProp, labelText, placeholderValue);
        }

        void Construct(JsProperty jsProp, string labelText)
        {
            LabelText = labelText;
            DefaultScripts = jsProp.DefaultScripts;
            OnValueChangedHandler = jsProp.OnValueChangedHandler;
            PropertyName = jsProp.PropertyName;
            CheckingScripts = jsProp.CheckingScripts;
        }

        void Construct(JsProperty jsProp, string labelText, string placeholderValue)
        {
            Construct(jsProp: jsProp, labelText: labelText);
            PlaceholderValue = placeholderValue;
        }
        #endregion

        /// <summary>
        /// Текст лейбла
        /// </summary>
        public string LabelText { get; set; }

        /// <summary>
        /// Плейсхолдер (предзаполненное значение)
        /// </summary>
        public string PlaceholderValue { get; set; }

        public JsInputDataType InputType { get; set; }
    }
}
