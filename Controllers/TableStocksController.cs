using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableStocksController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableStocks
        public ActionResult Index(string ticker = "", int sector = 0, int department = 0)
        {
            var tableStock = db.TableStock.Where(x => x.ticker == ticker || ticker == "")
                .Where(x => x.sectorId == sector || sector == 0)
                .Where(x => x.departmentId == department || department == 0)
                .Include(t => t.TableCurrency).Include(t => t.TableDepartment).Include(t => t.TableExchange).Include(t => t.TableSector);
            return View(tableStock.ToList());
        }

        // GET: TableStocks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableStock tableStock = db.TableStock.Find(id);
            if (tableStock == null)
            {
                return HttpNotFound();
            }
            return View(tableStock);
        }

        // GET: TableStocks/Create
        public ActionResult Create()
        {
            ViewBag.currencyCode = new SelectList(db.TableCurrency, "code", "name");
            ViewBag.departmentId = new SelectList(db.TableDepartment, "Id", "name");
            ViewBag.exchangeCode = new SelectList(db.TableExchange, "code", "name");
            ViewBag.sectorId = new SelectList(db.TableSector, "Id", "name");
            return View();
        }

        // POST: TableStocks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ticker,sectorId,departmentId,name,lot,currencyCode,exchangeCode,note,pe,ps,debtEq,price,priceW52High,priceW52Low")] TableStock tableStock)
        {
            if (ModelState.IsValid)
            {
                db.TableStock.Add(tableStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.currencyCode = new SelectList(db.TableCurrency, "code", "name", tableStock.currencyCode);
            ViewBag.departmentId = new SelectList(db.TableDepartment, "Id", "name", tableStock.departmentId);
            ViewBag.exchangeCode = new SelectList(db.TableExchange, "code", "name", tableStock.exchangeCode);
            ViewBag.sectorId = new SelectList(db.TableSector, "Id", "name", tableStock.sectorId);
            return View(tableStock);
        }

        // GET: TableStocks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableStock tableStock = db.TableStock.Find(id);
            if (tableStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.currencyCode = new SelectList(db.TableCurrency, "code", "name", tableStock.currencyCode);
            ViewBag.departmentId = new SelectList(db.TableDepartment, "Id", "name", tableStock.departmentId);
            ViewBag.exchangeCode = new SelectList(db.TableExchange, "code", "name", tableStock.exchangeCode);
            ViewBag.sectorId = new SelectList(db.TableSector, "Id", "name", tableStock.sectorId);
            return View(tableStock);
        }

        // POST: TableStocks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ticker,sectorId,departmentId,name,lot,currencyCode,exchangeCode,note,pe,ps,debtEq,price,priceW52High,priceW52Low")] TableStock tableStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.currencyCode = new SelectList(db.TableCurrency, "code", "name", tableStock.currencyCode);
            ViewBag.departmentId = new SelectList(db.TableDepartment, "Id", "name", tableStock.departmentId);
            ViewBag.exchangeCode = new SelectList(db.TableExchange, "code", "name", tableStock.exchangeCode);
            ViewBag.sectorId = new SelectList(db.TableSector, "Id", "name", tableStock.sectorId);
            return View(tableStock);
        }

        // GET: TableStocks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableStock tableStock = db.TableStock.Find(id);
            if (tableStock == null)
            {
                return HttpNotFound();
            }
            return View(tableStock);
        }

        // POST: TableStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TableStock tableStock = db.TableStock.Find(id);
            db.TableStock.Remove(tableStock);
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

        // GET: TableStocks/UpdDivs/IBM
        public ActionResult UpdDivs(string ticker)
        {
            if (ticker == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = Task.Run(() => PortfolioUtils.getContent(ticker));
            task.Wait();
            return RedirectToRoute(new { controller = "TableDividends", action = "Index" });
        }
    }
}
