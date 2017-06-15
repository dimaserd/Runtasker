
using System.Web.Mvc;

namespace Runtasker.Controllers
{
    [Authorize]
    public class BugBountyController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}