using System.Collections.Generic;

namespace Runtasker.Logic.Entities.ClickLinks
{
    public class CountingClickLink
    {
        public CountingClickLink()
        {
            Clicks = new List<Click>();
        }

        public string Id { get; set; }

        public string ClickName { get; set; }

        public string Description { get; set; }

        public ICollection<Click> Clicks { get; set; }
    }

    public class CountingClickLinkModel
    {
        public string Id { get; set; }

        public string ClickName { get; set; }

        public string Description { get; set; }

        public int ClicksCount { get; set; }
    }
}
