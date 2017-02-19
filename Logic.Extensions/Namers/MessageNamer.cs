
namespace Logic.Extensions.Namers
{
    public class MessageNamer
    {
        public string GetForMessageAboutOrder(int orderId)
        {
            return orderId.ToString();
        }
    }
}
