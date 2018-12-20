using MusicPortal.Infrastructure;
using MusicPortal.Models;
using MusicPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Controllers
{
    public class UsersController : Controller
    {
        private MusicPortalDbContext db = new MusicPortalDbContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);


            return View(
                (await db.Users
                            .ToListAsync())
                            .Select(u => u.ToViewModel()));
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new UserVM()
            {
                Id = user.Id,
                Login = user.Login,
                Roles = user.Roles.Select(r => new SelectedRole()
                {
                    Name = r.Name,
                    IsSelected = true
                })
            });
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return RedirectToAction("Register", "User");
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user.ToViewModel(await db.UserRoles.ToListAsync()));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Login,Roles")] UserVM userVM)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);


            if (userVM == null || !ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Unexpected token error");
                return View(userVM);
            }

            User dbUser = await db.Users.FirstAsync(u => u.Id == userVM.Id);
            dbUser.Roles.Clear();

            await Task.Run(() =>
            {
                userVM
                    .Roles
                    .Where(r => r.IsSelected)
                    .Select(async r => await db.UserRoles.FindAsync(r.Id))
                    .Select(task => task.Result)
                    .ToList()
                    .ForEach(role => dbUser.Roles.Add(role));
            });

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
