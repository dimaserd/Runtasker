
namespace Runtasker.Logic.Workers.MessageWorker
{

    public interface IMesageChatHub
    {
        UIHubMessage SendMessage(object message);
        //Sends message to base and return UIHubMessage to ChatHub
        //Than ChatHub sends information to users group
    }
}
