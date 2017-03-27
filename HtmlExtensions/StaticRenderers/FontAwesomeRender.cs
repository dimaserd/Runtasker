using HtmlExtensions.Renderers;

namespace HtmlExtensions.StaticRenderers
{
    //<i class="fa fa-rub" aria-hidden="true"></i>
    
    public static class FASigns
    {
        #region Public Properties
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


}
