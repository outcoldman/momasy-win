using System;
using System.Collections.Generic;
using System.Linq;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.Services
{
    internal interface IDictionaryEntityService<TInterface>
    {
        event Action CollectionChanged;

        IEnumerable<TInterface> GetAll();
        void Add(TInterface entity);
        void Update(TInterface entity);
        void Save(TInterface entity);
        void Delete(TInterface entity);
        TInterface Get(TInterface entity);
        TInterface Get(int id);
    }

    internal abstract class DictionaryServiceBase<TInterface, TEntity> : ServiceBase<TInterface, TEntity>, IDictionaryEntityService<TInterface>
        where TEntity : Entity<TEntity>
    {

        private IList<TEntity> _items;
        private object _sync = new object();

        private Func<TEntity, bool> _whereCondition;

        protected DictionaryServiceBase(IoC ioc)
            : base(ioc)
        {
        }

        public event Action CollectionChanged;

        public virtual IEnumerable<TInterface> GetAll()
        {
            LoadItems();

            // return copy of list
            return _items.Select(x => CreateContainer(x.Clone())).ToList();
        }

        public virtual void Add(TInterface item)
        {
            var dataItem = GetDataItem(item);

            if (!dataItem.IsNew)
                throw new ArgumentException(string.Format("Item {0} should be unsaved instance.", typeof(TEntity)), "item");

            using (var context = GetContext())
            {
                context.Set<TEntity>().Add(dataItem);
                context.SaveChanges();
            }
            RaiseChanged();
        }

        public virtual void Update(TInterface item)
        {
            var dataItem = GetDataItem(item);

            if (dataItem.IsNew)
                throw new ArgumentException(string.Format("Item {0} should be modified instance.", typeof(TEntity)), "item");

            using (var context = GetContext())
            {
                context.AttachAsModified(dataItem);
                context.SaveChanges();
            }
            RaiseChanged();
        }

        public virtual void Save(TInterface item)
        {
            var service = (IDictionaryEntityService<TInterface>)this;

            var dataItem = GetDataItem(item);

            if (dataItem.IsNew)
                service.Add(item);
            else
                service.Update(item);
        }

        public virtual void Delete(TInterface item)
        {
            var dataItem = GetDataItem(item);

            if (dataItem.IsNew)
                throw new ArgumentException(string.Format("Item {0} should be saved to database instance.", typeof(TEntity)), "item");

            using (var context = GetContext())
            {
                context.AttachAsModified(dataItem);
                context.Set<TEntity>().Remove(dataItem);
                context.SaveChanges();
            }
            RaiseChanged();
        }

        public virtual TInterface Get(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Identity should be greater than 0.", "id");

            var items = _items;

            if (items == null)
                throw new InvalidOperationException(string.Format("Collection of items {0} should be loaded first.", typeof(TEntity)));

            return CreateContainer(items.SingleOrDefault(x => x.Id == id));
        }

        public virtual TInterface Get(TInterface item)
        {
            var dataItem = GetDataItem(item);

            return ((IDictionaryEntityService<TInterface>)this).Get(dataItem.Id);
        }

        protected abstract TInterface CreateContainer(TEntity entity);

        protected void SetWhereCondition(Func<TEntity, bool> whereCondition)
        {
            if (whereCondition == null)
                throw new ArgumentNullException("whereCondition");

            lock (_sync)
            {
                if (whereCondition != _whereCondition)
                    _whereCondition = whereCondition;
                if (_items != null)
                    RaiseChanged();
            }
        }

        private void LoadItems()
        {
            if (_items == null)
            {
                lock (_sync)
                {
                    if (_items == null)
                    {
                        using (var context = GetContext())
                        {
                            var query = (IEnumerable<TEntity>)context.Set<TEntity>();
                            if (_whereCondition != null)
                                query = query.Where(_whereCondition);
                            _items = query.ToList();
                        }
                    }
                }
            }
        }

        private void RaiseChanged()
        {
            lock (_sync)
            {
                _items = null;
            }

            var changed = CollectionChanged;
            if (changed != null)
                changed();
        }
    }
}
