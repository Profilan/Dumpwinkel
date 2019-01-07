using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Persister.Entity;

namespace Dumpwinkel.Logic.Data
{
    internal class EventListener :
        IPreInsertEventListener,
        IPreUpdateEventListener
    {
        private static readonly string ModificationDatePropertyName = GetPropertyName<ISystemInfo>(val => val.Modified),
                                       CreationDatePropertyName = GetPropertyName<ISystemInfo>(val => val.Created);
        private static readonly string CreatorPropertyName = GetPropertyName<ISystemInfo>(val => val.CreatedBy),
                                       ModifierPropertyName = GetPropertyName<ISystemInfo>(val => val.ModifiedBy);

        public bool OnPreInsert(PreInsertEvent ev)
        {
            ISystemInfo entity = ev.Entity as ISystemInfo;
            if (entity != null)
            {
                var currentDate = DateTime.Now;
                entity.Created = entity.Modified = currentDate;
                SetState(ev.Persister, ev.State, ModificationDatePropertyName, currentDate);
                SetState(ev.Persister, ev.State, CreationDatePropertyName, currentDate);
            }
            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent ev, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public bool OnPreUpdate(PreUpdateEvent ev)
        {
            ISystemInfo entity = ev.Entity as ISystemInfo;
            if (entity != null)
            {
                var currentDate = DateTime.Now;
                entity.Modified = currentDate;
                SetState(ev.Persister, ev.State, ModificationDatePropertyName, currentDate);
            }
            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent ev, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private void SetState(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

        private static string GetPropertyName<TType>(Expression<Func<TType, object>> expression)
        {
            return ExpressionProcessor.FindPropertyExpression(expression.Body);
        }
    }
}
