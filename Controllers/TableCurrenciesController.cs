using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableCurrenciesController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableCurrencies
        public ActionResult Index()
        {
            return View(db.TableCurrency.ToList());
        }

        // GET: TableCurrencies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableCurrency tableCurrency = db.TableCurrency.Find(id);
            if (tableCurrency == null)
            {
                return HttpNotFound();
            }
            return View(tableCurrency);
        }

        // GET: TableCurrencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableCurrencies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "code,name")] TableCurrency tableCurrency)
        {
            if (ModelState.IsValid)
            {
                db.TableCurrency.Add(tableCurrency);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableCurrency);
        }

        // GET: TableCurrencies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableCurrency tableCurrency = db.TableCurrency.Find(id);
            if (tableCurrency == null)
            {
                return HttpNotFound();
            }
            return View(tableCurrency);
        }

        // POST: TableCurrencies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "code,name")] TableCurrency tableCurrency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableCurrency).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableCurrency);
        }

        // GET: TableCurrencies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableCurrency tableCurrency = db.TableCurrency.Find(id);
            if (tableCurrency == null)
            {
                return HttpNotFound();
            }
            return View(tableCurrency);
        }

        // POST: TableCurrencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TableCurrency tableCurrency = db.TableCurrency.Find(id);
            db.TableCurrency.Remove(tableCurrency);
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
