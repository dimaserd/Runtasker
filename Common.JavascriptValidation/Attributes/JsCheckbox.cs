using System;

namespace Common.JavascriptValidation.Attributes
{
    /// <summary>
    /// Аттрибут показывающий что данное свойство является чекбоксом.
    /// Так как document.getElementById("id").value не работает.
    /// Нужно использовать document.getElementById("id").checked.
    /// </summary>
    public class JsCheckBox : Attribute
    {

    }
}
