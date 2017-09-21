using Extensions.String;
using oksoft.Common.HtmlExtensions.Entities;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace oksoft.Common.HtmlExtensions.Static
{
    public static class FASigns
    {
        #region Файлы иконок
        /// <summary>
        /// Иконка обозначающая файл 
        /// </summary>
        public static FontAwesomeModel File
        {
            get
            {
                return new FontAwesomeModel("file");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл 
        /// </summary>
        public static FontAwesomeModel ExcelFile
        {
            get
            {
                return new FontAwesomeModel("file-excel-o");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл 
        /// </summary>
        public static FontAwesomeModel PdfFile
        {
            get
            {
                return new FontAwesomeModel("file-pdf-o");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл 
        /// </summary>
        public static FontAwesomeModel SoundFile
        {
            get
            {
                return new FontAwesomeModel("file-sound-o");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл Word
        /// </summary>
        public static FontAwesomeModel WordFile
        {
            get
            {
                return new FontAwesomeModel("file-word-o");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл изображения
        /// </summary>
        public static FontAwesomeModel PictureFile
        {
            get
            {
                return new FontAwesomeModel("file-picture-o");
            }
        }

        /// <summary>
        /// Иконка обозначающая файл 
        /// </summary>
        public static FontAwesomeModel VideoFile
        {
            get
            {
                return new FontAwesomeModel("file-video-o");
            }
        }
        #endregion

        #region Социальные сети
        public static FontAwesomeModel Facebook
        {
            get
            {
                return new FontAwesomeModel("facebook");
            }
        }

        public static FontAwesomeModel Twitter
        {
            get
            {
                return new FontAwesomeModel("twitter");
            }
        }

        public static FontAwesomeModel Google
        {
            get
            {
                return new FontAwesomeModel("google");
            }
        }

        public static FontAwesomeModel Instagram
        {
            get
            {
                return new FontAwesomeModel("instagram");
            }
        }

        public static FontAwesomeModel Vk
        {
            get
            {
                return new FontAwesomeModel("vk");
            }
        }

        public static FontAwesomeModel Youtube
        {
            get
            {
                return new FontAwesomeModel("youtube");
            }
        }

        public static FontAwesomeModel MailRu
        {
            get
            {
                return new FontAwesomeModel("mail-ru");
            }
        }

        public static FontAwesomeModel Odnoklassniki
        {
            get
            {
                return new FontAwesomeModel("odnoklassniki");
            }
        }
        #endregion

        #region Стрелки
        public static FontAwesomeModel List
        {
            get
            {
                return new FontAwesomeModel("list");
            }
        }

        public static FontAwesomeModel Plus
        {
            get
            {
                return new FontAwesomeModel("plus");
            }
        }

        public static FontAwesomeModel PlusCircle
        {
            get
            {
                return new FontAwesomeModel("plus-circle");
            }
        }

        /// <summary>
        /// Стрелка вправо
        /// </summary>
        public static FontAwesomeModel ChevronRight
        {
            get
            {
                return new FontAwesomeModel("chevron-right");
            }
        }

        /// <summary>
        /// Стрелка влево
        /// </summary>
        public static FontAwesomeModel ChevronLeft
        {
            get
            {
                return new FontAwesomeModel("chevron-left");
            }
        }

        /// <summary>
        /// Стрелка вверх
        /// </summary>
        public static FontAwesomeModel ChevronUp
        {
            get
            {
                return new FontAwesomeModel("chevron-up");
            }
        }

        /// <summary>
        /// Стрелка вниз
        /// </summary>
        public static FontAwesomeModel ChevronDown
        {
            get
            {
                return new FontAwesomeModel("chevron-down");
            }
        }


        /// <summary>
        /// Стрелка вправо. Внутри кружка
        /// </summary>
        public static FontAwesomeModel ChevronCircleRight
        {
            get
            {
                return new FontAwesomeModel("chevron-circle-right");
            }
        }

        /// <summary>
        /// Стрелка влево. Внутри кружка
        /// </summary>
        public static FontAwesomeModel ChevronCircleLeft
        {
            get
            {
                return new FontAwesomeModel("chevron-circle-left");
            }
        }

        /// <summary>
        /// Стрелка вверх. Внутри кружка
        /// </summary>
        public static FontAwesomeModel ChevronCircleUp
        {
            get
            {
                return new FontAwesomeModel("chevron-circle-up");
            }
        }

        /// <summary>
        /// Стрелка вниз. Внутри кружка
        /// </summary>
        public static FontAwesomeModel ChevronCircleDown
        {
            get
            {
                return new FontAwesomeModel("chevron-circle-down");
            }
        }
        #endregion

        #region Карандаш
        /// <summary>
        /// Темная иконка карандаша
        /// </summary>
        public static FontAwesomeModel Pencil
        {
            get
            {
                return new FontAwesomeModel("pencil");
            }
        }

        /// <summary>
        /// Карандаш в квадрате
        /// </summary>
        public static FontAwesomeModel PencilSquare
        {
            get
            {
                return new FontAwesomeModel("pencil-square");
            }
        }

        /// <summary>
        /// Карандаш в прозрачном квадрате 
        /// </summary>
        public static FontAwesomeModel PencilSquareTransparent
        {
            get
            {
                return new FontAwesomeModel("pencil-square-o");
            }
        }
        #endregion

        #region Звездочка
        /// <summary>
        /// Темная иконка изображающая зведочку
        /// </summary>
        public static FontAwesomeModel Star
        {
            get
            {
                return new FontAwesomeModel("star");
            }
        }

        /// <summary>
        /// Прозрачная иконка изображающая зведочку 
        /// </summary>
        public static FontAwesomeModel StarTransparent
        {
            get
            {
                return new FontAwesomeModel("star-o");
            }
        }
        #endregion


        /// <summary>
        /// Иконка с крестиками (используется для показа закрытия) 
        /// </summary>
        public static FontAwesomeModel Times
        {
            get
            {
                return new FontAwesomeModel("times");
            }
        }

        /// <summary>
        /// Темная иконка с тремя точками 
        /// </summary>
        public static FontAwesomeModel Commenting 
        {
            get
            {
                return new FontAwesomeModel("commenting");
            }
        }


        /// <summary>
        /// Иконка c двумя окнами комментариев
        /// </summary>
        public static FontAwesomeModel Comments
        {
            get
            {
                return new FontAwesomeModel("comments");
            }
        }



        /// <summary>
        /// Иконка знака вопроса (в кружке)
        /// </summary>
        public static FontAwesomeModel QuestionCircle
        {
            get
            {
                return new FontAwesomeModel("question-circle");
            }
        }


        /// <summary>
        /// Значок с галочкой (используется для выбора)
        /// </summary>
        public static FontAwesomeModel CheckMark
        {
            get
            {
                return new FontAwesomeModel("check");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static FontAwesomeModel CheckSquare
        {
            get
            {
                return new FontAwesomeModel("check-square");
            }
        }

        /// <summary>
        /// Значок урны
        /// </summary>
        public static FontAwesomeModel Trash
        {
            get
            {
                return new FontAwesomeModel("trash");
            }
        }


        /// <summary>
        /// Иконка знака вопроса
        /// </summary>
        public static FontAwesomeModel Question
        {
            get
            {
                return new FontAwesomeModel("question");
            }
        }

        /// <summary>
        /// Иконка буквы i (в кружке) - обозначающая получение информации
        /// </summary>
        public static FontAwesomeModel InfoCircle
        {
            get
            {
                return new FontAwesomeModel("info-circle");
            }
        }

        /// <summary>
        /// Иконка буквы i - обозначающая получение информации
        /// </summary>
        public static FontAwesomeModel Info
        {
            get
            {
                return new FontAwesomeModel("info");
            }
        }


        /// <summary>
        /// Иконка корзины с плюсиком(можно использовать на кнопке добавления товара в корзину)
        /// </summary>
        public static FontAwesomeModel CartPlus
        {
            get
            {
                return new FontAwesomeModel("cart-plus");
            }
        }

        /// <summary>
        /// Иконка корзины со стрелочкой вниз(можно использовать на кнопке добавления товара в корзину)
        /// </summary>
        public static FontAwesomeModel CartArrowDown
        {
            get
            {
                return new FontAwesomeModel("cart-arrow-down");
            }
        }


        /// <summary>
        /// Иконка корзины
        /// </summary>
        public static FontAwesomeModel ShoppingCart
        {
            get
            {
                return new FontAwesomeModel("shopping-cart");
            }
        }


        /// <summary>
        /// Иконка темного круга
        /// </summary>
        public static FontAwesomeModel Circle
        {
            get
            {
                return new FontAwesomeModel("circle");
            }
        }

        /// <summary>
        /// Иконка прозрачного квадрата
        /// </summary>
        public static FontAwesomeModel SquareO
        {
            get
            {
                return new FontAwesomeModel("square-o");
            }
        }

        /// <summary>
        /// Значок бумажного самолета используется для отправки сообщений или подписки на новости 
        /// </summary>
        public static FontAwesomeModel PaperPlane
        {
            get
            {
                return new FontAwesomeModel("paper-plane");
            }
        }

        public static FontAwesomeModel SadFace
        {
            get
            {
                return new FontAwesomeModel("frown-o"); 
            }
        }

        public static FontAwesomeModel Male
        {
            get
            {
                return new FontAwesomeModel("male");
            }
        }

        /// <summary>
        /// Единократная стрелочка влево.
        /// </summary>
        public static FontAwesomeModel AngleLeft
        {
            get
            {
                return new FontAwesomeModel("angle-left");
            }
        }

        /// <summary>
        /// Значок новостей
        /// </summary>
        public static FontAwesomeModel NewsPaper
        {
            get
            {
                return new FontAwesomeModel("newspaper-o");
            }
        }

        /// <summary>
        /// Иконка лупы
        /// </summary>
        public static FontAwesomeModel Search
        {
            get
            {
                return new FontAwesomeModel("search");
            }
        }

        /// <summary>
        /// Иконка стрелочки вниз [используется для раскрывающий список]
        /// </summary>
        public static FontAwesomeModel AngleDown
        {
            get
            {
                return new FontAwesomeModel("angle-down");
            }
        }


        /// <summary>
        /// Иконка стрелочки вверх
        /// </summary>
        public static FontAwesomeModel ArrowCicrcleUp
        {
            get
            {
                return new FontAwesomeModel("arrow-circle-up");
            }
        }

        /// <summary>
        /// Иконка с тремя пользователями
        /// </summary>
        public static FontAwesomeModel Users
        {
            get
            {
                return new FontAwesomeModel("users");
            }
        }


        /// <summary>
        /// Иконка восклицательного знака в треугольнике
        /// </summary>
        public static FontAwesomeModel Warning
        {
            get
            {
                return new FontAwesomeModel("warning");
            }
        }

        /// <summary>
        /// Иконка со временем
        /// </summary>
        public static FontAwesomeModel History
        {
            get
            {
                return new FontAwesomeModel("history");
            }
        }
    }

    public static class FontAwesomeExtensions
    {
        public static FontAwesomeModel GetFileIcon(string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                return FASigns.File;
            }
            string ext = fileName.Split(separator: new string[] { "." }, options: System.StringSplitOptions.RemoveEmptyEntries).Last();
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            switch (ext)
            {
                case "doc":
                    return FASigns.WordFile;

                case "docx":
                    return FASigns.WordFile;

                case "xlx":
                    return FASigns.ExcelFile;

                case "xlsx":
                    return FASigns.ExcelFile;

                case "pdf":
                    return FASigns.PdfFile;

                default:
                    if (mimeType.StartsWith("image"))
                    {
                        return FASigns.PictureFile;
                    }
                    else if(mimeType.StartsWith("audio"))
                    {
                        return FASigns.SoundFile;
                    }
                    else if(mimeType.StartsWith("video"))
                    {
                        return FASigns.VideoFile;
                    }
                    else
                    {
                        return FASigns.File;
                    }
            }
        }

        public static MvcHtmlString WrapToSquare(this FontAwesomeModel model)
        {
            //            <span class="fa-stack fa-lg">
            //              <i class="fa fa-square-o fa-stack-2x"></i>
            //              <i class="fa fa-twitter fa-stack-1x"></i>
            //            </span>

            FontAwesomeModel squareO = FASigns.SquareO;
            squareO.AfterFaClass += " fa-stack-2x";

            model.AfterFaClass += " fa-stack-1x";


            return new MvcHtmlString($"{squareO}{model}".WrapToHtmlTag("span", attributes: new { @class = "fa-stack fa-lg" }));
        }

        /// <summary>
        /// Устанавливает Id для элемента 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static FontAwesomeModel SetId(this FontAwesomeModel model, string id)
        {
            return new FontAwesomeModel(model.AfterFaClass, id);
        }

        public static FontAwesomeModel AddClass(this FontAwesomeModel model, string @class)
        {
            return new FontAwesomeModel(model.AfterFaClass + $" {@class}");
        }

        public static string GetClass(this FontAwesomeModel model)
        {
            return model.AfterFaClass;
        }
    }
}
