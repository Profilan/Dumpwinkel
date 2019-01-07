using Dumpwinkel.Logic.Data;
using Dumpwinkel.Logic.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dumpwinkel.Logic.Repositories
{
    public class DumpstoreRepository : Profilan.SharedKernel.IRepository<Dumpstore, Guid>
    {
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Dumpstore GetById(Guid id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Dumpstore>(id);
                    
                    return item;
                }
            }
        }

        public void Insert(Dumpstore entity)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Dumpstore> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Dumpstore> List()
        {
            throw new NotImplementedException();
        }

        public void Update(Dumpstore entity)
        {
            throw new NotImplementedException();
        }
    }
}
