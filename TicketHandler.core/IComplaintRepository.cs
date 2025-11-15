using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHandler.core
{
    public interface IComplaintRepository
    {
        public void Add(Complaints complain);

        public void Update(Complaints complain);

        public void Delete(int id);

        public IEnumerable<Complaints> GetAllComplaints();

        public Complaints FindByID(int id);
    }
}
