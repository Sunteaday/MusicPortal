using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicPortal.Models;
using MusicPortal.Infrastructure;

namespace MusicPortal.Controllers
{
    public class GenresController : Controller
    {
        private MusicPortalDbContext db = new MusicPortalDbContext();

        // GET: Genres
        public async Task<ActionResult> Index()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(await db.Genres.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Genre genre)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Genre genre)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            Genre genre = await db.Genres.FindAsync(id);
            db.Genres.Remove(genre);
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
