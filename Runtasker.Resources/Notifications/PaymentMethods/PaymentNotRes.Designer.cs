﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Notifications.PaymentMethods {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PaymentNotRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PaymentNotRes() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Runtasker.Resources.Notifications.PaymentMethods.PaymentNotRes", typeof(PaymentNotRes).Assembly);
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
        ///   Ищет локализованную строку, похожую на {0}Create Order.
        /// </summary>
        public static string CreateOrderFormat {
            get {
                return ResourceManager.GetString("CreateOrderFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на We have received your payment {0}{1} {2}..
        /// </summary>
        public static string PaymentReceivedTitleFormat {
            get {
                return ResourceManager.GetString("PaymentReceivedTitleFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на {0}Pay Order #{1}.
        /// </summary>
        public static string PayOrderFormat {
            get {
                return ResourceManager.GetString("PayOrderFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на via RoboKassa service.
        /// </summary>
        public static string ViaRoboKassa {
            get {
                return ResourceManager.GetString("ViaRoboKassa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на via YandexKassa service.
        /// </summary>
        public static string ViaYandexKassa {
            get {
                return ResourceManager.GetString("ViaYandexKassa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на via YandexMoney service.
        /// </summary>
        public static string ViaYandexMoney {
            get {
                return ResourceManager.GetString("ViaYandexMoney", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Your balance is {0}{1}..
        /// </summary>
        public static string YourBalanceFormat {
            get {
                return ResourceManager.GetString("YourBalanceFormat", resourceCulture);
            }
        }
    }
}
