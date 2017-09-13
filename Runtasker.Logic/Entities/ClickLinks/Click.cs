using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities.ClickLinks
{
    public class Click
    {
        public string Id { get; set; }

        public string PreviousUrl { get; set; }

        public string Info { get; set; }

        public DateTime ClickDate { get; set; }

        [ForeignKey("Link")]
        public string CountingClickLinkId { get; set; }

        public CountingClickLink Link { get; set; }
    }
}
