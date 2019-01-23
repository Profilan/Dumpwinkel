using Dumpwinkel.Logic.Data;
using Dumpwinkel.Logic.Models;
using NHibernate;
using System;
using System.Collections.Generic;

namespace Dumpwinkel.Logic.Repositories
{
    public class SettingRepository : Profilan.SharedKernel.IRepository<Setting, int>
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Setting GetById(int id)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var item = session.Get<Setting>(id);

                    return item;
                }
            }
        }

        public void Insert(Setting entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Setting> List(string sortOrder, string searchString, int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Setting> List()
        {
            throw new NotImplementedException();
        }

        public void Update(Setting entity)
        {
            using (ISession session = SessionFactory.GetNewSession("default"))
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
