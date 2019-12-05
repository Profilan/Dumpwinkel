using Dumpwinkel.Logic.Models;
using NHibernate;
using Profilan.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Repositories
{
    public class ScanRepository : IRepository<Scan, int>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Scan GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Scan entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Scan> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Scan> List()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Scan> ListByRegistration(Guid registrationId)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                var query = from x in session.Query<Scan>()
                            select x;

                query = query.Where(x => x.Registration.Id == registrationId);
                query = query.OrderBy(x => x.Timestamp);

                return query.ToList();
            }
        }

        public void Update(Scan entity)
        {
            throw new NotImplementedException();
        }
    }
}
