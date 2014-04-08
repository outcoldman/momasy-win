using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using outcold.MoMaSy.Data.DataModel;

namespace outcold.MoMaSy.Data
{
    internal class StorageContext : DbContext
    {
        public StorageContext(Storage storage)
            : base(storage.GetConnection(), true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<CurrencyType>().Property(x => x.Id).HasColumnName("CurrencyTypeId");
            modelBuilder.Entity<Account>().Property(x => x.Id).HasColumnName("AccountId");
            modelBuilder.Entity<TransactionType>().Property(x => x.Id).HasColumnName("TransactionTypeId");
            modelBuilder.Entity<Transaction>().Property(x => x.Id).HasColumnName("TransactionId");

            base.OnModelCreating(modelBuilder);
        } 


        public DbSet<CurrencyType> CurrencyTypes { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public void AttachAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
