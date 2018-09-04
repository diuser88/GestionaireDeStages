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
 

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                bool rolcontrol = false;
                var lesRoles = _context.Roles;


                foreach (var nomRole in lesRoles)
                {
                    if (User.IsInRole(nomRole.Name))
                    {
                        rolcontrol = true;
                    }
                }

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
            ViewData["Message"] = "Your contact page.";



            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
