﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Notifications.CustomerOrderErrorMethods {
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
    public class CustOrderErrorRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CustOrderErrorRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Runtasker.Resources.Notifications.CustomerOrderErrorMethods.CustOrderErrorRes", typeof(CustOrderErrorRes).Assembly);
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
        ///   Looks up a localized string similar to Ваш Баланс : {0}{1}. Для оплаты заказа №{2} требуется {3}{1}. Вам необходимо пополнить ваш счет на {4}{1}..
        /// </summary>
        public static string PayWithoutMoneyTextFormat {
            get {
                return ResourceManager.GetString("PayWithoutMoneyTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У вас недостаточно средств!.
        /// </summary>
        public static string PayWithoutMoneyTitle {
            get {
                return ResourceManager.GetString("PayWithoutMoneyTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Пополнить счет на {0}{1}.
        /// </summary>
        public static string TopUpBalanceFormat {
            get {
                return ResourceManager.GetString("TopUpBalanceFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Проверьте вашу почту, вам должно прийти электронное письмо. Вы не можете делать заказы, пока вы не подтвердили свою электронную почту!.
        /// </summary>
        public static string UnconfirmedAttemptText {
            get {
                return ResourceManager.GetString("UnconfirmedAttemptText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to У вас не подтвержден адрес электронной почты!.
        /// </summary>
        public static string UnconfirmedAttemptTitle {
            get {
                return ResourceManager.GetString("UnconfirmedAttemptTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ваш Баланс.
        /// </summary>
        public static string YourBalance {
            get {
                return ResourceManager.GetString("YourBalance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ваш Баланс : {0}{1}..
        /// </summary>
        public static string YourBalanceFormat {
            get {
                return ResourceManager.GetString("YourBalanceFormat", resourceCulture);
            }
        }
    }
}
