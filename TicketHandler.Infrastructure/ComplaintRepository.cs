//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TicketHandler.core;
//using TicketHandler.Infrastructure.Data;

//namespace TicketHandler.Infrastructure
//{
//    public class ComplaintRepository : IComplaintRepository
//    {
//        private readonly UserDbContext DbContext;
//        public ComplaintRepository(UserDbContext DbContext)
//        {
//            this.DbContext = DbContext;
//        }
//        public void Add(Complaints complain)
//        {
//            DbContext.Complaints.AddAsync(complain);
//            DbContext.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            Complaints cmp = DbContext.Complaints.Find(id);
//            DbContext.Remove(cmp);
//            DbContext.SaveChanges();
//        }

//        public void Edit(Complaints complain)
//        {
//            DbContext.Entry(complain).State=EntityState.Modified;
//            DbContext.SaveChanges();
//        }

//        public Complaints FindByID(int id)
//        {
//            Complaints cmp=(from r in DbContext.Complaints where r.ComplaintId==id select r).FirstOrDefault();
//            return cmp;
//        }

//        public List<Complaints> GetAllUser()
//        {
//            return DbContext.Complaints.ToList();
//        }
//    }
//}
