using System;
using System.Collections.Generic;
using System.Linq;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.Services
{
    internal abstract class ServiceBase<TInterface, TEntity>
        where TEntity : Entity<TEntity>
    {
        private IoC _ioc;

        protected ServiceBase(IoC ioc)
        {
            _ioc = ioc;
        }

        protected IoC IoC
        {
            get { return _ioc; }
        }

        protected TEntity GetDataItem(TInterface item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var entityModel = item as IEntityModel<TEntity>;

            if (entityModel == null)
                throw new ArgumentException("item");

            return entityModel.GetDataItem();
        }

        protected StorageContext GetContext()
        {
            return _ioc.Resolve<StorageContext>();
        }
    }
}
