using Runtasker.Resources.Models.OrderModels.CreateOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaScriptValidation.Test.Models
{
    public class TestModel
    {
        [Required(ErrorMessageResourceType = typeof(CreateOrder), ErrorMessageResourceName = "OtherSubjectError")]
        public string RequiredProp { get; set; }
    }
}
