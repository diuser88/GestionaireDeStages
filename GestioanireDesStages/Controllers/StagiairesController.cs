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
    public class StagiairesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StagiairesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Stagiaires
        [Authorize(Roles = "Administrateur, Superviseur")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Personne> stagiaires = null;

            stagiaires = _context.Personnes.Where(p => (p.Stagiaire));

            return View(await stagiaires.ToListAsync());
        }

        // GET: Stagiaires/Details/5
        [Authorize(Roles = "Administrateur, Superviseur")]
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

        // GET: Stagiaires/Create
        //[Authorize(Roles = "Administrateur, Superviseur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stagiaires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Create([Bind("PersonneId,Nom,Telephone,Courriel,Administrateur,Superviseur,Stagiaire")] Personne personne)
        {
            if (ModelState.IsValid)
            {
                personne.Stagiaire = true;
                _context.Add(personne);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }

        // GET: Stagiaires/Edit/5
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

        // POST: Stagiaires/Edit/5
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
                    UtilisateursController utilisateur = new UtilisateursController(_context, _userManager);
                    await utilisateur.AddRols(personne);
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

        // GET: Stagiaires/Delete/5
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

        // POST: Stagiaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personne = await _context.Personnes.SingleOrDefaultAsync(m => m.PersonneId == id);
            var user = await _userManager.FindByEmailAsync(personne.Courriel);
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
