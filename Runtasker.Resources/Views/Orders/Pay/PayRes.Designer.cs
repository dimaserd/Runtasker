﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Views.Orders.Pay {
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
    public class PayRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PayRes() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Runtasker.Resources.Views.Orders.Pay.PayRes", typeof(PayRes).Assembly);
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
        ///   Ищет локализованную строку, похожую на Home.
        /// </summary>
        public static string HomeNav {
            get {
                return ResourceManager.GetString("HomeNav", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на My Orders.
        /// </summary>
        public static string MyOrdersNav {
            get {
                return ResourceManager.GetString("MyOrdersNav", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay.
        /// </summary>
        public static string Pay {
            get {
                return ResourceManager.GetString("Pay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0} Pay.
        /// </summary>
        public static string PayBtnFormat {
            get {
                return ResourceManager.GetString("PayBtnFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay half price of order #{0}.
        /// </summary>
        public static string PayHalfFormat {
            get {
                return ResourceManager.GetString("PayHalfFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на On this page you&apos;re paying a first half of your order&apos;s cost. When you&apos;re done with this, the performer starts executing your work. {0}{1} will be debited from your account..
        /// </summary>
        public static string PayHelpTextFormat {
            get {
                return ResourceManager.GetString("PayHelpTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay Online-Help.
        /// </summary>
        public static string PayOnlineHelpTitle {
            get {
                return ResourceManager.GetString("PayOnlineHelpTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay {0}{1}.
        /// </summary>
        public static string PayRoublesFormat {
            get {
                return ResourceManager.GetString("PayRoublesFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Pay second half of order #{0}.
        /// </summary>
        public static string PaySecondHalfFormat {
            get {
                return ResourceManager.GetString("PaySecondHalfFormat", resourceCulture);
            }
        }
    }
}
