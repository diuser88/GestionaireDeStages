using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestioanireDesStages.Data;
using GestioanireDesStages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GestioanireDesStages.Controllers
{
    public class UtilisateursController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UtilisateursController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Utilisateurs
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personnes.ToListAsync());                                           
        }

        // GET: Utilisateurs/Details/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // GET: Utilisateurs/Create
        [Authorize(Roles = "Administrateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Create([Bind("PersonneId,Nom,Telephone,Courriel,Administrateur,Superviseur,Stagiaire")] Personne personne)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personne);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }

        // GET: Utilisateurs/Edit/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes.SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }
            return View(personne);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int id, [Bind("PersonneId,Nom,Prenom,Telephone,Courriel,Administrateur,Superviseur,Stagiaire")] Personne personne)
        {
            if (id != personne.PersonneId)
            {
                return NotFound();
            }

                if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personne);
                    await _context.SaveChangesAsync();
                    await AddRols(personne);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonneExists(personne.PersonneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }


        //Changement de role avec les checkbox
        [Authorize(Roles = "Administrateur, Superviseur")]
        public async Task AddRols(Personne p_personne)
        {
            ApplicationUser user = null;
            if (p_personne.Administrateur)
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if(!await _userManager.IsInRoleAsync(user, "Administrateur"))
                {
                    await _userManager.AddToRoleAsync(user, "Administrateur");
                }
            }
            else
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if (await _userManager.IsInRoleAsync(user, "Administrateur"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Administrateur");
                }
            }

            if (p_personne.Superviseur)
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if (!await _userManager.IsInRoleAsync(user, "Superviseur"))
                {
                    await _userManager.AddToRoleAsync(user, "Superviseur");
                }
            }
            else
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if (await _userManager.IsInRoleAsync(user, "Superviseur"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Superviseur");
                }
            }

            if (p_personne.Stagiaire)
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if (!await _userManager.IsInRoleAsync(user, "Stagiaire"))
                {
                    await _userManager.AddToRoleAsync(user, "Stagiaire");
                }
            }
            else
            {
                user = await _userManager.FindByEmailAsync(p_personne.Courriel);
                if (await _userManager.IsInRoleAsync(user, "Stagiaire"))
                {
                    await _userManager.RemoveFromRoleAsync(user, "Stagiaire");
                }
            }

            if(user == null)
            {
                user = new ApplicationUser();
                await _userManager.AddToRoleAsync(user, "Stagiaire");
            }
        }


        // GET: Utilisateurs/Delete/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personne = await _context.Personnes.SingleOrDefaultAsync(m => m.PersonneId == id);
            ApplicationUser user = await _userManager.FindByEmailAsync(personne.Courriel);
            foreach(var stage in _context.Stages.Where(s => s.Stagiaire == personne.PersonneId))
            {
                _context.Stages.Remove(stage);
            }
            _context.Personnes.Remove(personne);
            await _userManager.DeleteAsync(user);
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PersonneExists(int id)
        {
            return _context.Personnes.Any(e => e.PersonneId == id);
        }
    }
}
