﻿using Microsoft.AspNetCore.Mvc;
using Tounaent_Fixtures.Models;

namespace Tounaent_Fixtures.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TournamentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(TournamentViewModel model)
        {

                var tournament = new TblTournament
                {
                    TournamentName = model.TournamentName,
                    OrganizedBy = model.OrganizedBy,
                    Venue = model.Venue,
                    FromDt = model.From_dt,
                    ToDt = model.To_dt,
                    AddedDt = DateTime.Now,
                    AddedBy = User.Identity?.Name ?? "admin",
                    IsActive = model.IsActive
                };

                _context.TblTournament.Add(tournament);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tournament successfully registered!";

                return View(new TournamentViewModel());


        }
    }

}
