﻿using System.Web.Mvc;

namespace HtmlExtensions.StaticRenderers
{
    //<i class="fa fa-rub" aria-hidden="true"></i>

    public static class FASigns
    {
        #region Свойства
        /// <summary>
        /// Значок стоячего мужика
        /// </summary>
        public static FontAwesomeSignModel Male
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-male"
                };
            }
        }

        /// <summary>
        /// Значок ключика
        /// </summary>
        public static FontAwesomeSignModel Key
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-key"
                };
            }
        }

        /// <summary>
        /// Значок с тремя пользователями
        /// </summary>
        public static FontAwesomeSignModel Users
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-users"
                };
            }
        }

        /// <summary>
        /// Значок пользователя
        /// </summary>
        public static FontAwesomeSignModel User
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-user"
                };
            }
        }

        /// <summary>
        /// Знак закрытого замочка
        /// </summary>
        public static FontAwesomeSignModel Lock
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-lock"
                };
            }
        }


        /// <summary>
        /// Знак банкноты (с единичкой)
        /// </summary>
        public static FontAwesomeSignModel Money
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-money"
                };
            }
        }

        /// <summary>
        /// Изображение айфона
        /// </summary>
        public static FontAwesomeSignModel MobilePhone
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-mobile"
                };
            }
        }
        /// <summary>
        /// Восклицательный знак
        /// </summary>
        public static FontAwesomeSignModel Exclamation
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-exclamation"
                };
            }
        }
        /// <summary>
        /// Стрелка показывающая на возвращение операции
        /// </summary>
        public static FontAwesomeSignModel Undo
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-undo"
                };
            }
        }

        public static FontAwesomeSignModel PlusSquare
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-plus-square"
                };
            }
        }

        public static FontAwesomeSignModel Configuration
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-cog"
                };
            }
        }

        public static FontAwesomeSignModel PlusCircle
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-plus-circle"
                };
            }
        }

        public static FontAwesomeSignModel CircleONotch
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-circle-o-notch"
                };
            }
        }


        public static FontAwesomeSignModel DoubleBack
        {
            get
            {
                return new FontAwesomeSignModel
                {
                    @class = "fa-angle-double-left"
                };
            }
        }

        public static FontAwesomeSignModel CreditCard
        {
            get
            {
                //return "<i class='fa fa-credit-card'></i>";
                return new FontAwesomeSignModel
                {
                    @class = "fa-credit-card"
                };
            }
        }

        public static FontAwesomeSignModel Rouble
        {
            get
            { //return "<i class='fa fa-rub'></i>"; 
                return new FontAwesomeSignModel
                {
                    @class = "fa-rub"
                };

            }
        }

        public static FontAwesomeSignModel Download
        {
            get
            { //return "<i class='fa fa-download'></i>"; 
                return new FontAwesomeSignModel
                {
                    @class = "fa-download"
                };
            }
        }

        public static FontAwesomeSignModel Info
        {
            get
            { //return "<i class='fa fa-info'></i>"; 
                return new FontAwesomeSignModel
                {
                    @class = "fa-info"
                };
            }
        }
        #endregion
    }

    public class FontAwesomeSignModel
    {
        #region Properties
        public string @class { get; set; }
        #endregion

        #region Overridden Methods
        public override string ToString()
        {
            return $"<i class='fa {@class}'></i>";
        }
        #endregion
    }

    public static class FontAwesomeExtensions
    {
        #region Size Changing Extensions
        public static FontAwesomeSignModel Lg(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-lg";
            return FASign;
        }

        public static FontAwesomeSignModel X2(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-2x";
            return FASign;
        }

        public static FontAwesomeSignModel X3(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-3x";
            return FASign;
        }

        public static FontAwesomeSignModel X4(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-4x";
            return FASign;
        }

        public static FontAwesomeSignModel X5(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-5x";
            return FASign;
        }
        #endregion

        public static FontAwesomeSignModel Animate(this FontAwesomeSignModel FASign)
        {
            FASign.@class = FASign.@class + " fa-spin";
            return FASign;
        }

        public static MvcHtmlString ToHtml(this FontAwesomeSignModel FASign)
        {
            return MvcHtmlString.Create(FASign.ToString());
        }


    }
}
