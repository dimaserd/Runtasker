using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Logic.Models.WokerResults
{
    public class PropertyError
    {
        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class WorkerPropertyResult
    {
        #region Constructors
        public WorkerPropertyResult()
        {
            ErrorsList = new List<PropertyError>();
        }

        public WorkerPropertyResult(List<PropertyError> errorsList)
        {
            Succeeded = false;
            ErrorsList = errorsList;
        }
        #endregion

        public bool Succeeded { get; set; }

        public List<PropertyError> ErrorsList { get; set; }
    }
}
