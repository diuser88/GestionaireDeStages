using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestioanireDesStages.Models;
using Microsoft.AspNetCore.Authorization;
using GestioanireDesStages.Data;
using Microsoft.AspNetCore.Identity;

namespace GestioanireDesStages.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public HomeController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public IActionResult Index()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                bool rolcontrol = false;
                var lesRoles = _roleManager.Roles;
                

                foreach (var nomRole in lesRoles)
                {
                    if (User.IsInRole(nomRole.Name))
                    {
                        rolcontrol = true;
                    }
                }
                //if (User.IsInRole("Stagiaire"))
                //{
                //    var test = _context.Personnes.FirstOrDefault(p => p.Courriel == User.Identity.Name);
                //    ViewData["PersonneId"] = test.PersonneId;
                //}

                if (!rolcontrol)
                {
                    return View(nameof(UtilisateurSansRole));
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UtilisateurSansRole()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "L'adresse";



            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
