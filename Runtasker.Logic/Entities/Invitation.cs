using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public InvitationStatus Status { get; set; }

        public string SenderGuid { get; set; }

        public string ReceiverEmail { get; set; }

        public string ReceiverGuid { get; set; }

        public DateTime Date { get; set; }
    }

    public enum InvitationStatus
    {
        Sent, UserRegistered, Paid
    }
}
