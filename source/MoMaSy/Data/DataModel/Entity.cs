using System;

namespace outcold.MoMaSy.Data.DataModel
{
    internal abstract class Entity<TEntity> : ICloneable
        where TEntity : Entity<TEntity>
    {
        public int Id { get; set; }

        public bool IsNew
        {
            get
            {
                return Id <= 0;
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public virtual TEntity Clone()
        {
            return MemberwiseClone() as TEntity;
        }
    }
}
