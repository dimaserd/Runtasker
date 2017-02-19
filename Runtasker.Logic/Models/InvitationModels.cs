
using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Models
{
    public class SendInvitationModel
    {
        public string UserGuid { get; set; }

        [EmailAddress]
        public string ReceiverEmail { get; set; }
    }

    
}
