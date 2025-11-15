using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketHandler.core;
using TicketHandler.Infrastructure.Data;
using TicketHandler.Infrastructure.Filters;

namespace TicketHandler.Web.Controllers
{
    public class UserdatasController : Controller
    {
        private readonly UserDbContext _context;

        public UserdatasController(UserDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(Login viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserData.Where(x => (x.Email == viewModel.Email) && (x.Password == viewModel.Password)).FirstOrDefault();

                if (user != null)
                {
                        HttpContext.Session.SetString("Uname", user.UserName);
                        HttpContext.Session.SetString("Role", user.Role);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role, user.Role)
                        };
                        var claimsIdentity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("Index", "Complaints");
                }
                else
                {
                    ViewBag.Message = "INVALID ENTRIES";
                    return View();
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("login","Userdatas");
        }
        // GET: Userdatas
        [CustomAuthorisation]
        public async Task<IActionResult> Index()
        {
            ViewBag.user = _context.UserData.Count(c=> c.Role=="User");
            ViewBag.agent = _context.UserData.Count(c => c.Role == "Agent");
            return View(await _context.UserData.ToListAsync());
        }

        // GET: Userdatas/Details/5
        [CustomAuthorisation]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            { 
                return NotFound();
            }

            var userdata = await _context.UserData
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userdata == null)
            {
                return NotFound();
            }

            return View(userdata);
        }
        public IActionResult Profile()
        {
            var uname = HttpContext.Session.GetString("Uname");
            var user = _context.UserData.FirstOrDefault(u => u.UserName == uname);
            var complaintCount = _context.Complaints.Count(c => c.UserName == uname);

            ViewBag.ComplaintCount = complaintCount;
            return View(user);
        }
        // GET: Userdatas/Create
        [CustomAuthorisation]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Userdatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,Email,Password,ConPassword,Role")] Userdata userdata)
        {
            if (ModelState.IsValid)
            {
                var check2 = await _context.UserData
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Email == userdata.Email);
                if (check2 != null)
                {
                    ViewBag.Message += "Email is already registered";
                    return View(userdata);
                }
                _context.Add(userdata);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userdata);
        }

        // GET: Userdatas/Edit/5
        [CustomAuthorisation]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userdata = await _context.UserData.FindAsync(id);
            if (userdata == null)
            {
                return NotFound();
            }
            return View(userdata);
        }

        // POST: Userdatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,First_Name,Last_Name,Email,EmpId,Role,Password,CreatedAt,isActive")] Userdata userdata)
        {
            if (id != userdata.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userdata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserdataExists(userdata.UserId))
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
            return View(userdata);
        }

        // GET: Userdatas/Delete/5
        [CustomAuthorisation]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userdata = await _context.UserData
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userdata == null)
            {
                return NotFound();
            }

            return View(userdata);
        }

        // POST: Userdatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userdata = await _context.UserData.FindAsync(id);
            if (userdata != null)
            {
                _context.UserData.Remove(userdata);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserdataExists(int id)
        {
            return _context.UserData.Any(e => e.UserId == id);
        }
    }
}
