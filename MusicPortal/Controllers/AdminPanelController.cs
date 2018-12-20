using MusicPortal.Infrastructure;
using MusicPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Controllers
{
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        public ActionResult Index()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View();
        }
    }
}