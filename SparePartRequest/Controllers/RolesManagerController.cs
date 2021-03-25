using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SparePartRequest.ViewModels;
using SparePartRequest.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SparePartRequest.Controllers
{
    public class RolesManagerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: RolesManager
        public ActionResult Index()
        {
            var systemRoles = db.Roles.ToList();
            List<RoleViewModel> roles = new List<RoleViewModel>();
            RoleViewModel role;

            
            foreach (IdentityRole sysrole in systemRoles)
            {
                role = new RoleViewModel();
                role.RoleName = sysrole.Name;

                roles.Add(role);
            }


            return View(roles);
        }
    }
}