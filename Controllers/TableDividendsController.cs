using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableDividendsController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableDividends
        public ActionResult Index()
        {
            var tableDividends = db.TableDividends.Include(t => t.TableStock);
            return View(tableDividends.ToList());
        }
    }
}
