using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Runtasker.Logic.Entities
{
    #region Extension methods

    #endregion

    public class OtherUserInfo
    {

        public string UserId { get; set; }

        public string VkDomain { get; set; }

        public string VkId { get; set; }

        public string Specialization { get; set; }


        
    }
}
