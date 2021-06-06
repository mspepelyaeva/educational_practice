using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class TableUsersController : Controller
    {
        private masterEntities db = new masterEntities();

        // GET: TableUsers
        public ActionResult Index()
        {
            return View(db.TableUser.ToList());
        }

        // GET: TableUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableUser tableUser = db.TableUser.Find(id);
            if (tableUser == null)
            {
                return HttpNotFound();
            }
            return View(tableUser);
        }

        // GET: TableUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TableUsers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,login,password,nikname,email")] TableUser tableUser)
        {
            if (ModelState.IsValid)
            {
                db.TableUser.Add(tableUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tableUser);
        }

        // GET: TableUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableUser tableUser = db.TableUser.Find(id);
            if (tableUser == null)
            {
                return HttpNotFound();
            }
            return View(tableUser);
        }

        // POST: TableUsers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,login,password,nikname,email")] TableUser tableUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tableUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tableUser);
        }

        // GET: TableUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TableUser tableUser = db.TableUser.Find(id);
            if (tableUser == null)
            {
                return HttpNotFound();
            }
            return View(tableUser);
        }

        // POST: TableUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TableUser tableUser = db.TableUser.Find(id);
            db.TableUser.Remove(tableUser);
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
