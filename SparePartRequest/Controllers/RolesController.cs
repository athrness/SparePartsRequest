using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using SparePartRequest.Models;

namespace SparePartRequest.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext context;

        public RolesController()
        {
            context = new ApplicationDbContext();
        }



        public ActionResult Show()
        {
            HelloMsg msg = new HelloMsg();
            msg.Display = "Hello";

            return View(msg);
        }


        // GET: Role
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }
        //  [Authorize(Roles ="Admin")]

        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }


        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}

namespace SparePartRequest.Models
{
    public class HelloMsg
    {
        public string Display { get; set; }
    }
}
