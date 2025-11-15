using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHandler.core
{
    public interface IUserRepository
    {
        public void Add(Userdata data);

        public void Update(Userdata data);

        public void Delete(int id);

        public IEnumerable<Userdata> GetAllUser();

        public Userdata FindByID(int id);
    }
}
