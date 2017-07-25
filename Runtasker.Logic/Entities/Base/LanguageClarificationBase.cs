using Runtasker.LocaleBuilders.Enumerations;

namespace Runtasker.Logic.Entities.Base
{
    /// <summary>
    /// Базовая сущность описывающая локализованные свойства объекта из базы данных
    /// 
    /// </summary>
    public class LanguageClarificationBase
    {
        /// <summary>
        /// Идентификатор языкового уточнения (идентификатор целочисленный, так как 
        /// всего уточнений не может быть больше чем, кол-во языков)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код языка для данного уточнения
        /// </summary>
        public Lang LanguageCode { get; set; }
    }
}
