using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Runtasker.Logic;
using Runtasker.Logic.Entities.ClickLinks;

namespace Runtasker.Controllers.Mvc
{
    [Authorize(Roles = "Admin")]
    public class ClicksController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Clicks
        public async Task<ActionResult> Index()
        {
            var query = from m in db.CountingClickLinks
                        select new CountingClickLinkModel
                        {
                            ClickName = m.ClickName,
                            ClicksCount = m.Clicks.Count(),
                            Description = m.Description,
                            Id = m.Id
                        };

            return View(await query.ToListAsync());
        }

        // GET: Clicks/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountingClickLink countingClickLink = await db.CountingClickLinks.Include(t => t.Clicks).FirstOrDefaultAsync(x => x.Id == id);
            if (countingClickLink == null)
            {
                return HttpNotFound();
            }
            return View(countingClickLink);
        }

        // GET: Clicks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clicks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CountingClickLink countingClickLink)
        {
            if (ModelState.IsValid)
            {
                countingClickLink.Id = Guid.NewGuid().ToString();

                db.CountingClickLinks.Add(countingClickLink);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(countingClickLink);
        }

        // GET: Clicks/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountingClickLink countingClickLink = await db.CountingClickLinks.FindAsync(id);
            if (countingClickLink == null)
            {
                return HttpNotFound();
            }
            return View(countingClickLink);
        }

        // POST: Clicks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CountingClickLink countingClickLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(countingClickLink).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(countingClickLink);
        }

        // GET: Clicks/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountingClickLink countingClickLink = await db.CountingClickLinks.FindAsync(id);
            if (countingClickLink == null)
            {
                return HttpNotFound();
            }
            return View(countingClickLink);
        }

        // POST: Clicks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            CountingClickLink countingClickLink = await db.CountingClickLinks.FindAsync(id);
            db.CountingClickLinks.Remove(countingClickLink);
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
