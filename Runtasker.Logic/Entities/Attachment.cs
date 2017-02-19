using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{

    public partial class Attachment
    {
        public Attachment()
        {
            Id = Guid.NewGuid().ToString();
        }
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string FileName { set; get; }

        public string Mark { get; set; }

        //[ForeignKey("Attachment")]
        //public virtual int? MessageId { get; set; }
        public virtual Message Message { get; set; }
        #endregion
    }
}