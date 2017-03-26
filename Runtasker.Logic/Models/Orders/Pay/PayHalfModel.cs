﻿using Runtasker.Resources.Models.CustomerOrderModels;
using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Models.Orders.Pay
{
    public class PayHalfModel
    {
        [Display(Name = "Sum", ResourceType = typeof(CustOrderModelsRes))]
        public decimal Sum { get; set; }

        public int OrderId { get; set; }

        public decimal RequiredSum { get; set; }
    }
}