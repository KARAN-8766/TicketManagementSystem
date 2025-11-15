using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketHandler.core;
using TicketHandler.Infrastructure.Data;
using TicketHandler.Infrastructure.Filters;
using TicketHandler.Infrastructure.Services;

namespace TicketHandler.Web.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly UserDbContext _context;

        private readonly TicketService _ticketService;

        public ComplaintsController(UserDbContext context, TicketService ticketService)
        {
            _context = context;
            _ticketService = ticketService;
        }

        // GET: Complaints
        [CustomAuthorisation]
        public async Task<IActionResult> Index()
        {
            string user = HttpContext.Session.GetString("Uname");
            string role = HttpContext.Session.GetString("Role");

            ViewBag.User = user;
            ViewBag.Role = role;
            ViewBag.New = 0;
            ViewBag.InProgress = 0;
            ViewBag.Resolved = 0;
            ViewBag.Total = 0;
            if (string.IsNullOrEmpty(user))
            {
                return View();
            }
            List<Complaints> records=null;
            if (role.Equals("User"))
            {
                records = await _context.Complaints
                .Where(c => c.UserName == user)
                .ToListAsync();
                ViewBag.New = _context.Complaints.Count(c => (c.Status == "New") && (c.UserName==user));
                ViewBag.InProgress = _context.Complaints.Count(c => (c.Status == "In Progress") && (c.UserName == user));
                ViewBag.Resolved= _context.Complaints.Count(c => (c.Status == "Closed") && (c.UserName == user));
                ViewBag.Total=ViewBag.New+ViewBag.InProgress+ViewBag.Resolved;
            }
            else if (role.Equals("Agent"))
            {
                records = await _context.Complaints
                .Where(c => c.AssignTo == user)
                .ToListAsync();
                ViewBag.New = _context.Complaints.Count(c => (c.Status == "New") && (c.AssignTo == user));
                ViewBag.InProgress = _context.Complaints.Count(c => (c.Status == "In Progress") && (c.AssignTo == user));
                ViewBag.Resolved = _context.Complaints.Count(c => (c.Status == "Closed") && (c.AssignTo == user));
                ViewBag.Total = ViewBag.New + ViewBag.InProgress + ViewBag.Resolved;
            }

            else
            {
                records = await _context.Complaints
                .ToListAsync();
                ViewBag.New = _context.Complaints.Count(c => c.Status == "New");
                ViewBag.InProgress = _context.Complaints.Count(c => c.Status == "In Progress");
                ViewBag.Resolved = _context.Complaints.Count(c => c.Status == "Closed");
                ViewBag.Total = ViewBag.New + ViewBag.InProgress + ViewBag.Resolved;
            }
                
            return View(records);
        }

        // GET: Complaints/Details/5
        [CustomAuthorisation]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaints = await _context.Complaints
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaints == null)
            {
                return NotFound();
            }

            return View(complaints);
        }
        [CustomAuthorisation]
        public async Task<IActionResult> ShowClosedRec()
        {
            ViewBag.Total = _context.closedTickets.Count();
            return View(await _context.closedTickets.ToListAsync());
        }
        [CustomAuthorisation]
        public async Task<IActionResult> DetailClosedRec(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaints = await _context.closedTickets
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaints == null)
            {
                return NotFound();
            }

            return View(complaints);
        }
        // GET: Complaints/Create
        [CustomAuthorisation]
        public IActionResult Create()
        {
            List<Userdata> users=_context.UserData.Where(c => (c.Role == "Admin") || (c.Role == "Agent")).ToList();
            ViewBag.Users=new SelectList(users,"UserName","UserName");
            //var model = new Complaints()
            //{
            //    Created_on = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm")),
            //    Expected_Date = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
            //}; 
            return View();
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComplaintId,Subject,Description,Created_on,Priority,Expected_Date,Status,UserName,AssignTo")] Complaints complaints)
        {
            try
            {
                var uname = HttpContext.Session.GetString("Uname");
                if (string.IsNullOrEmpty(uname))
                {
                    TempData["ErrorMessage"] = "Your session has expired. Please login again.";
                    return RedirectToAction("Login", "UserData");
                }

                complaints.UserName = uname;
                complaints.Created_on = DateTime.Now; // ensure timestamp is set

                ModelState.Remove(nameof(complaints.UserName));

                if (ModelState.IsValid)
                {
                    _context.Add(complaints);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Complaint created successfully.";
                    return RedirectToAction("Index", "Complaints");
                }
                return View(complaints);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Create(): " + ex.Message);
                Console.WriteLine(ex.StackTrace);

                TempData["ErrorMessage"] = "Something went wrong while saving your complaint.";
                return View(complaints);
            }
        }

        // GET: Complaints/Edit/5
        [CustomAuthorisation]
        public async Task<IActionResult> Edit(int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            ViewBag.Role = role;
            if (id == null)
            {
                return NotFound();
            }

            var complaints = await _context.Complaints.FindAsync(id);
            if (complaints == null)
            {
                return NotFound();
            }
            return View(complaints);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComplaintId,Subject,Description,Created_on,Priority,Expected_Date,Status,UserName,Employee_Name,AssignTo")] Complaints complaints)
        {
            

            var uname = await _context.Complaints
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.ComplaintId == id);

            complaints.Created_on = DateTime.Now;
            
            ModelState.Remove(nameof(complaints.UserName));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complaints);
                    await _context.SaveChangesAsync();
                    if (complaints.Status == "Closed")
                    {
                        await _ticketService.UpdateTicketStatusAsync(complaints.ComplaintId, "Closed");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplaintsExists(complaints.ComplaintId))
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
            return View(complaints);
        }

        // GET: Complaints/Delete/5
        [CustomAuthorisation]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaints = await _context.Complaints
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaints == null)
            {
                return NotFound();
            }

            return View(complaints);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Complaints complaint)
        {
            var complaints = await _context.Complaints.FindAsync(complaint.ComplaintId);
            if (complaints != null)
            {
                _context.Complaints.Remove(complaints);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplaintsExists(int id)
        {
            return _context.Complaints.Any(e => e.ComplaintId == id);
        }
    }
}
