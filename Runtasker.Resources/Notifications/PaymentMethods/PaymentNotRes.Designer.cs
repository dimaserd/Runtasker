﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Notifications.PaymentMethods {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class PaymentNotRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal PaymentNotRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
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
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to {0}Создать заказ.
        /// </summary>
        public static string CreateOrderFormat {
            get {
                return ResourceManager.GetString("CreateOrderFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Мы получили вашу оплату {0}{1} {2}..
        /// </summary>
        public static string PaymentReceivedTitleFormat {
            get {
                return ResourceManager.GetString("PaymentReceivedTitleFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}Оплатить заказ №{1}.
        /// </summary>
        public static string PayOrderFormat {
            get {
                return ResourceManager.GetString("PayOrderFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to через Робокассу.
        /// </summary>
        public static string ViaRoboKassa {
            get {
                return ResourceManager.GetString("ViaRoboKassa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to через ЯндексКассу.
        /// </summary>
        public static string ViaYandexKassa {
            get {
                return ResourceManager.GetString("ViaYandexKassa", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to через ЯндексДеньги.
        /// </summary>
        public static string ViaYandexMoney {
            get {
                return ResourceManager.GetString("ViaYandexMoney", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ваш баланс : {0}{1}..
        /// </summary>
        public static string YourBalanceFormat {
            get {
                return ResourceManager.GetString("YourBalanceFormat", resourceCulture);
            }
        }
    }
}
