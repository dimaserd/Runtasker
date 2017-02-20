﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VkParser.Entities
{
    public class VkGroup
    {
        #region Constructors
        public VkGroup()
        {
            Posts = new List<VkFoundPost>();
        }
        #endregion

        [Key]
        public int Id { get; set; }

        

        public string ScreenName { get; set; }

        public string Name { get; set; }

        public DateTime LastCheckDate { get; set; }

        public int LastCheckedPostId { get; set; }

        public int GroupId { get; set; }

        //может быть отправлена заявка на участие в сообществе
        public bool IsMember { get; set; }


        public virtual ICollection<VkFoundPost> Posts { get; set; }
    }
}