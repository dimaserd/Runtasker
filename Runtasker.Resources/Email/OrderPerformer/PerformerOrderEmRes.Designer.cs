﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Email.OrderPerformer {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PerformerOrderEmRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PerformerOrderEmRes() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Runtasker.Resources.Email.OrderPerformer.PerformerOrderEmRes", typeof(PerformerOrderEmRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Go to my orders!.
        /// </summary>
        public static string ErrorFoundButtonText {
            get {
                return ResourceManager.GetString("ErrorFoundButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на about order #{0}.
        /// </summary>
        public static string ErrorFoundSubjectFormat {
            get {
                return ResourceManager.GetString("ErrorFoundSubjectFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Hello {0} we have found an error in your order #{1}. Please correct it. It’s easy..
        /// </summary>
        public static string ErrorFoundTextFormat {
            get {
                return ResourceManager.GetString("ErrorFoundTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay a half of it!.
        /// </summary>
        public static string EstimatedBtnText {
            get {
                return ResourceManager.GetString("EstimatedBtnText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Your order #{0} has been estimated.
        /// </summary>
        public static string EstimatedSubjectFormat {
            get {
                return ResourceManager.GetString("EstimatedSubjectFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Hello {0} we have estimated your order #{1} at {2}{3}. You need to pay half of the sum before we start carrying out your order..
        /// </summary>
        public static string EstimatedTextFormat {
            get {
                return ResourceManager.GetString("EstimatedTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay and download!.
        /// </summary>
        public static string ExecutedButtonText {
            get {
                return ResourceManager.GetString("ExecutedButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Your order #{0} has been executed.
        /// </summary>
        public static string ExecutedSubjectFormat {
            get {
                return ResourceManager.GetString("ExecutedSubjectFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Hello {0} we have completed the work on your order #{1}. You will be able to download the completed solution after paying the second half of the price - {2}{3}..
        /// </summary>
        public static string ExecutedTextFormat {
            get {
                return ResourceManager.GetString("ExecutedTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Hello {0}!.
        /// </summary>
        public static string GreetingFormat {
            get {
                return ResourceManager.GetString("GreetingFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Best wishes from the Runtasker team!.
        /// </summary>
        public static string RuntaskerWish {
            get {
                return ResourceManager.GetString("RuntaskerWish", resourceCulture);
            }
        }
    }
}
