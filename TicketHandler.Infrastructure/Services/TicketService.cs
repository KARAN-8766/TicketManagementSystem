using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHandler.core;
using TicketHandler.Infrastructure.Data;

namespace TicketHandler.Infrastructure.Services
{
    public class TicketService
    {
        private readonly UserDbContext _context;
        public TicketService(UserDbContext _context)
        {
            this._context = _context;
        }

        public async Task UpdateTicketStatusAsync(int id,string status)
        {
            var user = await _context.Complaints
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.ComplaintId == id);

            if (status=="Closed" && user != null)
            {
                var addrecord = new ClosedTicket
                {
                    UserName = user.UserName,
                    Created_on = user.Created_on,
                    Expected_Date = user.Expected_Date,
                    AssignTo = user.AssignTo,
                    Priority = user.Priority,
                    Subject = user.Subject,
                    Description = user.Description,
                };
                await _context.closedTickets.AddAsync(addrecord);
                await _context.SaveChangesAsync();
            }
        }
    }
}
