namespace Runtasker.Settings.Controllers
{
    /// <summary>
    /// Статический класс инкапсулирующий некоторые настройки контроллера File
    /// такие как ссылки на скачивание различных файлов и так далее
    /// </summary>
    public static class FileControllerSettings
    {
        public static string GetLinkForDownloadingCustomerFilesFromOrder(int orderId)
        {
            return $"/File/GetCustomerFiles?orderId={orderId}";
        }
    }
}
