using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Runtasker.Logic;
using Runtasker.Logic.Entities;
using Runtasker.Controllers.Base;
using System.Linq;
using Runtasker.LocaleBuilders.Enumerations;

namespace Runtasker.Controllers.Mvc
{
    [Authorize(Roles = "Admin")]
    public class QuestionAnswersController : BaseMvcController
    {
        private MyDbContext db = new MyDbContext();

        // GET: QuestionAnswers
        public async Task<ActionResult> Index()
        {
            var model = await db.QuestionAnswers.Include(x => x.Clarifications).ToListAsync();
            return View(model);
        }

        public async Task<ActionResult> Show()
        {
            Lang currentLang = CurrentLang;

            var model = await (from q in db.QuestionAnswerLangClarifications
                         where q.LanguageCode == currentLang && q.IsVisible
                         select q).ToListAsync();

            return View(model);
        }

        // GET: QuestionAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers.Include(x => x.Clarifications)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionAnswers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(enableValidation: false)]
        public async Task<ActionResult> Create(QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                questionAnswer.Clarifications.ToList()
                    .ForEach(x => db.QuestionAnswerLangClarifications.Add(x));

                db.QuestionAnswers.Add(questionAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers.Include(x => x.Clarifications)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(enableValidation: false)]
        public async Task<ActionResult> Edit(QuestionAnswer questionAnswer)
        {
            if (ModelState.IsValid)
            {
                foreach(QuestionAnswerLangClarification clarification in questionAnswer.Clarifications)
                {
                    db.Entry(clarification).State = EntityState.Modified;
                }
                db.Entry(questionAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(questionAnswer);
        }

        // GET: QuestionAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionAnswer questionAnswer = await db.QuestionAnswers
                .Include(x => x.Clarifications)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: QuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            QuestionAnswer questionAnswer = await db.QuestionAnswers
                .Include(x => x.Clarifications)
                .FirstOrDefaultAsync(x => x.Id == id);

            //foreach(QuestionAnswerLangClarification clarification in questionAnswer.Clarifications)
            //{
            //    db.QuestionAnswerLangClarifications.Remove(clarification);
            //}
            db.QuestionAnswers.Remove(questionAnswer);

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
