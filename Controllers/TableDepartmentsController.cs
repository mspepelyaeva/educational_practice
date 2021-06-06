using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableDepartmentsController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableDepartments
        public ActionResult Index()
        {
            return View(db.TableDepartment.ToList());
        }

        // GET: TableDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableDepartment tableDepartment = db.TableDepartment.Find(id);
            if (tableDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tableDepartment);
        }

        // GET: TableDepartments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableDepartments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,note")] TableDepartment tableDepartment)
        {
            if (ModelState.IsValid)
            {
                db.TableDepartment.Add(tableDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableDepartment);
        }

        // GET: TableDepartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableDepartment tableDepartment = db.TableDepartment.Find(id);
            if (tableDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tableDepartment);
        }

        // POST: TableDepartments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,note")] TableDepartment tableDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableDepartment);
        }

        // GET: TableDepartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableDepartment tableDepartment = db.TableDepartment.Find(id);
            if (tableDepartment == null)
            {
                return HttpNotFound();
            }
            return View(tableDepartment);
        }

        // POST: TableDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableDepartment tableDepartment = db.TableDepartment.Find(id);
            db.TableDepartment.Remove(tableDepartment);
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
