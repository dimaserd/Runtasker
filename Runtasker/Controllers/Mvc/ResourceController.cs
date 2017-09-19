using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Runtaker.LocaleBuiders.Entities;
using Runtaker.LocaleBuiders.Workers;
using Runtasker.Controllers.Base;

namespace Runtasker.Controllers.Mvc
{
    public class ResourceController : BaseMvcController
    {
        
        public async Task<ActionResult> Ged()
        {
            List<ResourceFileModel> all = ResourceModelCreator.GetModels().ToList();
            
            Db.ResourceFileModels.AddRange(all);

            await Db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Resource
        public async Task<ActionResult> Index()
        {
            return View(await Db.ResourceFileModels.ToListAsync());
        }

        // GET: Resource/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceFileModel resourceFileModel = await Db.ResourceFileModels.FindAsync(id);
            if (resourceFileModel == null)
            {
                return HttpNotFound();
            }
            return View(resourceFileModel);
        }

        #region Create
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
        public async Task<ActionResult> Create(ResourceFileModel resourceFileModel)
        {
            if (ModelState.IsValid)
            {
                Db.ResourceFileModels.Add(resourceFileModel);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(resourceFileModel);
        }

        #endregion

        #region Переводческие методы
        [HttpGet]
        public ActionResult Translate(string name)
        {
            ResourceFileModel resModel = Db.ResourceFileModels.Include(t => t.ResourceStrings.Select(x => x.ResourceStringTypes))
                .FirstOrDefault(t => t.ResourcePath == name);


            return View(resModel);
        }


        #endregion
        // GET: Resource/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceFileModel resourceFileModel = await Db.ResourceFileModels.FindAsync(id);
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
        public async Task<ActionResult> Edit(ResourceFileModel resourceFileModel)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(resourceFileModel).State = EntityState.Modified;
                await Db.SaveChangesAsync();
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
            ResourceFileModel resourceFileModel = await Db.ResourceFileModels.FindAsync(id);
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
            ResourceFileModel resourceFileModel = await Db.ResourceFileModels.FindAsync(id);
            Db.ResourceFileModels.Remove(resourceFileModel);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
    }
}
