﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Runtasker.Resources.Notifications.CustomerOrderErrorMethods {
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
    public class CustOrderErrorRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CustOrderErrorRes() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
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
        ///   Ищет локализованную строку, похожую на Your Balance : {0}{1}. Half price of order #{2} costs {3}{1}. You need to top up your balance by {4}{1}..
        /// </summary>
        public static string PayWithoutMoneyTextFormat {
            get {
                return ResourceManager.GetString("PayWithoutMoneyTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на You don&apos;t have enough money on your account!.
        /// </summary>
        public static string PayWithoutMoneyTitle {
            get {
                return ResourceManager.GetString("PayWithoutMoneyTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Top up by {0}{1}.
        /// </summary>
        public static string TopUpBalanceFormat {
            get {
                return ResourceManager.GetString("TopUpBalanceFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Wait for a confirmation email. You can not make an order while you&apos;re not confirmed!.
        /// </summary>
        public static string UnconfirmedAttemptText {
            get {
                return ResourceManager.GetString("UnconfirmedAttemptText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на You have not confirmed your email!.
        /// </summary>
        public static string UnconfirmedAttemptTitle {
            get {
                return ResourceManager.GetString("UnconfirmedAttemptTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Your Balance.
        /// </summary>
        public static string YourBalance {
            get {
                return ResourceManager.GetString("YourBalance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Your Balance : {0}{1}..
        /// </summary>
        public static string YourBalanceFormat {
            get {
                return ResourceManager.GetString("YourBalanceFormat", resourceCulture);
            }
        }
    }
}
