using MusicPortal.Infrastructure;
using MusicPortal.Models;
using MusicPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Controllers
{
    public class UserController : Controller
    {
        protected MusicPortalDbContext dbContext = new MusicPortalDbContext();

        // GET: User
        public ActionResult Index()
        {
            return View("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterConfirm(RegisterVM registerModel)
        {
            // if some the hacking actions with form. Or someone disable JS?
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Invalid token");

                return View(registerModel);
            }
            // check for already registred
            if (await dbContext.Users.Where(u => registerModel.Login == u.Login).CountAsync() > 0)
            {
                ModelState.AddModelError(String.Empty, "Login is already taken(and pass maybe too?)");
                return View(registerModel);
            }

            string salt = CryptoService.GetRandomBytes(AppConstants.PASSOWORD_SALT_LENGTH).ToHexString();

            byte[] password = Encoding.Unicode.GetBytes(salt + registerModel.Password);

            string hash = CryptoService.ComputeMD5Hash(password).ToHexString();

            User newUser = new User()
            {
                Login = registerModel.Login,
                Password = hash,
                Salt = salt
            };
            newUser.Roles.Add(await dbContext.UserRoles.Where(role => role.Name == "Authenticated").FirstAsync());

            dbContext.Users.Add(newUser);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginConfirm(LoginVM loginVM)
        {
            // if some the hacking actions with form or someione don't use JS
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Invalid token");
                return View(new LoginVM() { Login = loginVM.Login });
            }

            User user = await dbContext.Users.Where(u => loginVM.Login == u.Login).FirstOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "User not found");
                return View(new LoginVM() { Login = loginVM.Login });
            }

            byte[] password = Encoding.Unicode.GetBytes(user.Salt + loginVM.Password);

            string hash = CryptoService.ComputeMD5Hash(password).ToHexString();

            if (user.Password != hash)
            {
                ModelState.AddModelError(String.Empty, "Incorrect login or/and pass");
                return View(new LoginVM() { Login = loginVM.Login });
            }

            IEnumerable<string> userRoles = user.Roles.Select(role => role.Name);
            Session["User"] = user;
            Session["Roles"] = userRoles;
            Session["isAdmin"] = userRoles.Contains("Admin");
            Session["UserMenu"] = MenuProvider.BuildMenu(userRoles);

            return Redirect(Url.Content("~/"));
        }

        public ActionResult Logout()
        {
            // Session.Abandon();
            Session.Remove("User");
            Session.Remove("Roles");
            Session.Remove("isAdmin");
            Session.Remove("UserMenu");

            return Redirect(Url.Content("~/"));
        }
    }
}