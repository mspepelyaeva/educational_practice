using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TablePortfoliosController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TablePortfolios
        public ActionResult Index()
        {
            int userId = @WebApplication1.Models.GlobalVariables.UserId;
            bool isAdmin = @WebApplication1.Models.GlobalVariables.IsAdmin;
            // Админ "видит" все записи, непривилегированный пользователь - только свои
            var tablePortfolio = db.TablePortfolio.Where(x => x.userId == userId || isAdmin).Include(t => t.TableStock).Include(t => t.TableUser);
            return View(tablePortfolio.ToList());
        }

        // GET: TablePortfolios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablePortfolio tablePortfolio = db.TablePortfolio.Find(id);
            if (tablePortfolio == null)
            {
                return HttpNotFound();
            }
            return View(tablePortfolio);
        }

        // GET: TablePortfolios/Create
        public ActionResult Create()
        {
            ViewBag.ticker = new SelectList(db.TableStock, "ticker", "name");
            // Установка ИД пользоватля в качестве значения по-умолчанию
            ViewBag.userId = new SelectList(db.TableUser, "Id", "login", @WebApplication1.Models.GlobalVariables.UserId);
            return View();
        }

        // POST: TablePortfolios/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,userId,ticker,count,dateBue,priceBue,dateSell,priceSell")] TablePortfolio tablePortfolio)
        {
            tablePortfolio.userId = GlobalVariables.UserId;
            if (ModelState.IsValid)
            {
                db.TablePortfolio.Add(tablePortfolio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ticker = new SelectList(db.TableStock, "ticker", "name", tablePortfolio.ticker);
            ViewBag.userId = new SelectList(db.TableUser, "Id", "login", tablePortfolio.userId);
            return View(tablePortfolio);
        }

        // GET: TablePortfolios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablePortfolio tablePortfolio = db.TablePortfolio.Find(id);
            if (tablePortfolio == null)
            {
                return HttpNotFound();
            }
            ViewBag.ticker = new SelectList(db.TableStock, "ticker", "name", tablePortfolio.ticker);
            ViewBag.userId = new SelectList(db.TableUser, "Id", "login", tablePortfolio.userId);
            return View(tablePortfolio);
        }

        // POST: TablePortfolios/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,userId,ticker,count,dateBue,priceBue,dateSell,priceSell")] TablePortfolio tablePortfolio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tablePortfolio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ticker = new SelectList(db.TableStock, "ticker", "name", tablePortfolio.ticker);
            ViewBag.userId = new SelectList(db.TableUser, "Id", "login", tablePortfolio.userId);
            return View(tablePortfolio);
        }

        // GET: TablePortfolios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TablePortfolio tablePortfolio = db.TablePortfolio.Find(id);
            if (tablePortfolio == null)
            {
                return HttpNotFound();
            }
            return View(tablePortfolio);
        }

        // POST: TablePortfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TablePortfolio tablePortfolio = db.TablePortfolio.Find(id);
            db.TablePortfolio.Remove(tablePortfolio);
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
