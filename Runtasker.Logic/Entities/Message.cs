using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Runtasker.Logic.Entities
{
    public enum MessageStatus
    {
        New, Read, Deleted
    }

    public enum MessageType
    {
        Ordinary, AboutOrder
    }

    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [ForeignKey("Sender")]
        public string SenderGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Sender { get; set; }


        [Required]
        [StringLength(128)]
        [ForeignKey("Receiver")]
        public string ReceiverGuid { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public MessageStatus Status { get; set; }

        [Required]
        public MessageType Type { set; get; }

        public string Mark { get; set; }

        [StringLength(512)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        //[ForeignKey("Attachment")]
        public string AttachmentId { get; set; }
        //public virtual Attachment Attachment { get; set; }

        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }

}