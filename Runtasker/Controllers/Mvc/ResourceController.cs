using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Runtaker.LocaleBuiders.Entities;
using Runtasker.Logic;

namespace Runtasker.Controllers.Mvc
{
    public class ResourceController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Resource
        public async Task<ActionResult> Index()
        {
            return View(await db.ResourceFileModels.ToListAsync());
        }

        // GET: Resource/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceFileModel resourceFileModel = await db.ResourceFileModels.FindAsync(id);
            if (resourceFileModel == null)
            {
                return HttpNotFound();
            }
            return View(resourceFileModel);
        }

        // GET: Resource/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resource/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ResourcePath,CreateDate,LangCode")] ResourceFileModel resourceFileModel)
        {
            if (ModelState.IsValid)
            {
                db.ResourceFileModels.Add(resourceFileModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(resourceFileModel);
        }

        // GET: Resource/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceFileModel resourceFileModel = await db.ResourceFileModels.FindAsync(id);
            if (resourceFileModel == null)
            {
                return HttpNotFound();
            }
            return View(resourceFileModel);
        }

        // POST: Resource/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ResourcePath,CreateDate,LangCode")] ResourceFileModel resourceFileModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resourceFileModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(resourceFileModel);
        }

        // GET: Resource/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceFileModel resourceFileModel = await db.ResourceFileModels.FindAsync(id);
            if (resourceFileModel == null)
            {
                return HttpNotFound();
            }
            return View(resourceFileModel);
        }

        // POST: Resource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ResourceFileModel resourceFileModel = await db.ResourceFileModels.FindAsync(id);
            db.ResourceFileModels.Remove(resourceFileModel);
            await db.SaveChangesAsync();
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
