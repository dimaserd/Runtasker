﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Views.Payment.Robokassa {
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
    public class RkassaRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RkassaRes() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Runtasker.Resources.Views.Payment.Robokassa.RkassaRes", typeof(RkassaRes).Assembly);
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
        ///   Ищет локализованную строку, похожую на Topping-up.
        /// </summary>
        public static string ActionDesc1 {
            get {
                return ResourceManager.GetString("ActionDesc1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Topping-up {0}{1}.
        /// </summary>
        public static string ActionDescFormat {
            get {
                return ResourceManager.GetString("ActionDescFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на via RoboKassa.
        /// </summary>
        public static string ActionDescMini {
            get {
                return ResourceManager.GetString("ActionDescMini", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Back to payment methods.
        /// </summary>
        public static string BackToPM {
            get {
                return ResourceManager.GetString("BackToPM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на You are about to top up your balance with RoboKassa. Please be advised that you will pay {0}{1} with the commission charged by RoboKassa. We recommend using YandexMoney as it charges no commission. Click “Pay” to continue..
        /// </summary>
        public static string InfoFormat {
            get {
                return ResourceManager.GetString("InfoFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Robokassa.
        /// </summary>
        public static string NavActive {
            get {
                return ResourceManager.GetString("NavActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Home.
        /// </summary>
        public static string NavHome {
            get {
                return ResourceManager.GetString("NavHome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Topping up.
        /// </summary>
        public static string NavPayments {
            get {
                return ResourceManager.GetString("NavPayments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Topping-up your balance.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
