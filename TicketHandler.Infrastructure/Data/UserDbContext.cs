using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketHandler.core;

namespace TicketHandler.Infrastructure.Data
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) 
        {
            Database.SetCommandTimeout(180);
        }
        public override void Dispose()
        {
            try
            {
                var connection = Database.GetDbConnection();
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch
            {
                // Ignore errors — prevents exceptions when context is already disposed
            }

            base.Dispose();
        }
        public DbSet<Userdata> UserData { get; set; }

        public DbSet<Complaints> Complaints { get; set; }

        public DbSet<ClosedTicket> closedTickets { get; set; }
    }
}
