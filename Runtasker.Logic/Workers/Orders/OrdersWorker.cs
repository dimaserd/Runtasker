using Runtasker.Logic.Workers.Files;

namespace Runtasker.Logic.Workers.Orders
{
    //Unliason this classes
    public class SuperOrdersWorker
    {
        #region Constructors
        public SuperOrdersWorker(string userGuid)
        {
            UserGuid = userGuid;
            
            _context = new MyDbContext();
            FileWorker = new SuperFileWorker(_context);
            
        }
        #endregion

        #region Private Fields
        private MyDbContext _context { get; set; }
        private SuperFileWorker FileWorker { get; set; }
        private string UserGuid { get; set; }
        #endregion

        #region Public Properties
        public PerformerOrderWorker Performer { get ;  private set; }
        public CustomerOrderWorker Customer { get; private set; }
        
        //public NotificationWorker Notificater { get; private set; }
        #endregion

    }

}
