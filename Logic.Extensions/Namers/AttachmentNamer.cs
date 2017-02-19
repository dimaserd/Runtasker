

namespace Logic.Extensions.Namers
{
    //This class gets or sets names for any attachment
    //Order attachment [order][$id]
    //Chat about order attachment [chat][order][$id]
    //just chat [chat][concatofguids]
    public class AttachmentNamer
    {
        #region Constructors
        public AttachmentNamer()
        {
            Mark = new Mark();
        }
        #endregion

        #region Properties
        public Mark Mark { get; private set; }
        #endregion

        #region Public Methods


        public string GetLinkForAttachmentsField(string key)
        {
            return $"/File/DownloadByKey?key={key}";
        }
        #endregion
    }

    public class Mark
    {


        #region Get Mark methods
        public string GetForOrder(int orderId)
        {
            return $"[order][{orderId}]";
        }

        public string GetForMessage(string messageId)
        {
            return $"[message][{messageId}]";
        }

        public string GetForMessageAboutOrder(int orderId)
        {
            return $"[message][order][{orderId}]";
        }

        public string GetForOrderSolution(int orderId)
        {
            return $"[solution][{orderId}]";
        }
        #endregion
    }


}
