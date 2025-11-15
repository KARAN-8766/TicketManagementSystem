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
//    public class UserRepository : IUserRepository
//    {
//        private readonly UserDbContext Dbcontext;
//        public UserRepository(UserDbContext Dbcontext)
//        {
//            this.Dbcontext = Dbcontext;
//        }
//        public void Add(Userdata data)
//        {
//            Dbcontext.UserData.AddAsync(data);
//            Dbcontext.SaveChanges();
//        }

//        public void Delete(int id)
//        {
//            Userdata dt = Dbcontext.UserData.Find(id);
//            Dbcontext.Remove(dt);
//            Dbcontext.SaveChanges();
//        }

//        public void Edit(Userdata data)
//        {
//            Dbcontext.Entry(data).State=EntityState.Modified;
//            Dbcontext.SaveChanges();
//        }

//        public Userdata FindByID(int id)
//        {
//            Userdata data = (from r in Dbcontext.UserData where r.UserId == id select r).FirstOrDefault();
//            return data;
//        }

//        public List<Userdata> GetAllUser()
//        {
//            return Dbcontext.UserData.ToList();
//        }
//    }
//}
