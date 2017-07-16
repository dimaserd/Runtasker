namespace Runtasker.Logic.Enumerations
{
    /// <summary>
    /// По данному перечислению определяется нужно ли записывать изменения сразу
    /// или сохранение изменений произойдет позднее в другом классе
    /// </summary>
    public enum SaveChangesType
    {
        /// <summary>
        /// Записать сейчас
        /// </summary>
        Now,

        /// <summary>
        /// Записать позже вручную одним вызовом
        /// </summary>
        Handled
    }
}
