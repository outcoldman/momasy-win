using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;

namespace outcold.MoMaSy.Model
{
    internal interface IEntityModel<TEntity>
        where TEntity : Entity<TEntity>
    {
        TEntity GetDataItem();
    }

    internal abstract class EntityModel<TEntity>: IEntityModel<TEntity>
        where TEntity : Entity<TEntity>
    {
        protected EntityModel(TEntity dataItem)
        {
            if (dataItem == null)
                throw new ArgumentNullException("dataEntity");

            DataItem = dataItem;
        }

        protected TEntity DataItem { get; private set; }

        TEntity IEntityModel<TEntity>.GetDataItem()
        {
            return DataItem;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Id == ((EntityModel<TEntity>)obj).Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ Id;
        }
                
        public int Id
        {
            get { return DataItem.Id; }
        }

        public bool IsNew
        {
            get { return DataItem.IsNew; }
        }
    }
}
