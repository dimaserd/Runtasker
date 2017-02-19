using Runtasker.Logic.Entities;
using System.Collections.Generic;
using System.Web;

namespace Runtasker.Logic.Models
{
    public class ChooseOrderModel
    {
        public int OrderId { get; set; }

        public string PerformerGuid { get; set; }
    }

    public class DetailsModel
    {
       
    }

    public class ValueOrderModel
    {
        public int OrderId { get; set; }

        public decimal Sum { get; set; }
    }

    public class AddErrorToOrderModel
    {
        public int OrderId { get; set; }

        public OrderErrorType ErrorType { get; set; }
    }

    public class SolveOrderModel
    {
        public int OrderId { get; set; }

        public string PerformerGuid { get; set; }

        public IEnumerable<HttpPostedFileBase> SolutionFiles {get;set;}
    }
}