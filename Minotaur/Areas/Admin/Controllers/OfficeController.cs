﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minotaur.DataAccess;
using Minotaur.Models;
using Minotaur.Models.Models;
using Minotaur.Models.SD;
using Newtonsoft.Json;

namespace Minotaur.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Roles.RoleAdmin)]
    public class OfficeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MinotaurUser> _userManager;

        public OfficeController(ApplicationDbContext db, UserManager<MinotaurUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Office()
        {
            return View();
        }

        public async Task<IActionResult> GetOffices()
        {
            var offices = _db.Offices.ToArrayAsync();

            return Json(new { data = offices });
        }

        [HttpPost]
        public async Task<IActionResult> AddOffice(string dataOffice)
        {
            if (dataOffice == null) { return BadRequest(new { error = "Неверно заполнены данные" }); }

            Office? office = JsonConvert.DeserializeObject<Office>(dataOffice);



            if (office != null)
            {
                await _db.Offices.AddAsync(office);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}