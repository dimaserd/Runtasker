namespace Runtasker.Logic.Interfaces.Identity
{
    public interface IVkUser
    {
        string VkDomain { get; set; }

        string VkId { get; set; }

        bool ShouldBeNotifictedInVk { get; set; } 
    }
}
