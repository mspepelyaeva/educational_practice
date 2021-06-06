using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplication1
{
    public class TableSectorsController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableSectors
        public ActionResult Index()
        {
            return View(db.TableSector.ToList());
        }

        // GET: TableSectors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableSector tableSector = db.TableSector.Find(id);
            if (tableSector == null)
            {
                return HttpNotFound();
            }
            return View(tableSector);
        }

        // GET: TableSectors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableSectors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,note")] TableSector tableSector)
        {
            if (ModelState.IsValid)
            {
                db.TableSector.Add(tableSector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableSector);
        }

        // GET: TableSectors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableSector tableSector = db.TableSector.Find(id);
            if (tableSector == null)
            {
                return HttpNotFound();
            }
            return View(tableSector);
        }

        // POST: TableSectors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,note")] TableSector tableSector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableSector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableSector);
        }

        // GET: TableSectors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableSector tableSector = db.TableSector.Find(id);
            if (tableSector == null)
            {
                return HttpNotFound();
            }
            return View(tableSector);
        }

        // POST: TableSectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableSector tableSector = db.TableSector.Find(id);
            db.TableSector.Remove(tableSector);
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
