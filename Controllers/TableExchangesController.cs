using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableExchangesController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableExchanges
        public ActionResult Index()
        {
            return View(db.TableExchange.ToList());
        }

        // GET: TableExchanges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableExchange tableExchange = db.TableExchange.Find(id);
            if (tableExchange == null)
            {
                return HttpNotFound();
            }
            return View(tableExchange);
        }

        // GET: TableExchanges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableExchanges/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "code,name,note")] TableExchange tableExchange)
        {
            if (ModelState.IsValid)
            {
                db.TableExchange.Add(tableExchange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableExchange);
        }

        // GET: TableExchanges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableExchange tableExchange = db.TableExchange.Find(id);
            if (tableExchange == null)
            {
                return HttpNotFound();
            }
            return View(tableExchange);
        }

        // POST: TableExchanges/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "code,name,note")] TableExchange tableExchange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableExchange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableExchange);
        }

        // GET: TableExchanges/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableExchange tableExchange = db.TableExchange.Find(id);
            if (tableExchange == null)
            {
                return HttpNotFound();
            }
            return View(tableExchange);
        }

        // POST: TableExchanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TableExchange tableExchange = db.TableExchange.Find(id);
            db.TableExchange.Remove(tableExchange);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
