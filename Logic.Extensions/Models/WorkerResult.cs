using System.Collections.Generic;

namespace Logic.Extensions.Models
{
    public class WorkerResult
    {
        #region Constructors
        public WorkerResult()
        {
            Construct();
        }

        public WorkerResult(string Error)
        {
            Construct();
            ErrorsList.Add(Error);
        }

        public WorkerResult(IEnumerable<string> errors)
        {
            Construct();
            foreach(string error in errors)
            {
                ErrorsList.Add(error);
            }
            
        }

        void Construct()
        {
            Succeeded = false;
            ErrorsList = new List<string>();
        }
        #endregion

        #region Private Fields

        #endregion

        #region Properties
        public bool Succeeded { get; set; }

        public List<string> ErrorsList { get; set; }
        #endregion
    }
}
