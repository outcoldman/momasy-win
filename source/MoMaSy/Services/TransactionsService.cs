using System.Linq;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Resources.Strings;
using outcold.MoMaSy.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;

namespace outcold.MoMaSy.Services
{
    internal class TransactionsFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public IEnumerable<IAccount> Accounts { get; set; }
        public bool ShowHidden { get; set; }
    }

    internal interface ITransactionsService 
    {
        bool CanAddTransaction(out string errorMessage);
        ITransaction GetNewTransaction();
        IEnumerable<ITransaction> GetAll(TransactionsFilter filter);

        void Save(ITransaction entity);
        void Delete(ITransaction entity);
    }

    internal class TransactionsService : ServiceBase<ITransaction, Transaction>, ITransactionsService
    {
        public TransactionsService(IoC ioc)
            : base (ioc)
        {
        }
              
        bool ITransactionsService.CanAddTransaction(out string errorMessage)
        {
            errorMessage = null;

            var currencyTypes = IoC.Resolve<ICurrencyTypesService>();
            var accountsService = IoC.Resolve<IAccountsService>();
            var transactionTypesService = IoC.Resolve<ITransactionTypesService>();

            // Has at least one active currency type
            if (currencyTypes.GetAll().All(x => x.IsHidden))
                errorMessage = StringResources.ErrMsg_Transaction_AtLeastOneCurrency;

            // Has at least one active account.
            else if (accountsService.GetAll().All(x => x.IsHidden))
                errorMessage = StringResources.ErrMsg_Transaction_AtLeastOneAccount;

            // Has at least one active transaction type.
            else if (!transactionTypesService.GetAll().Any(x => x.Parent == null && !x.IsHidden))
                errorMessage = StringResources.ErrMsg_Transaction_AtLeastOneTransactionType;
            
            return errorMessage == null;
        }

        public ITransaction GetNewTransaction()
        {
            Transaction transaction = new Transaction();
            TransactionModel model = new TransactionModel(IoC, transaction);
            model.Date = DateTime.Today;
            model.TransactionKind = TransactionKind.Expense;
            var transactionTypesService = IoC.Resolve<ITransactionTypesService>();
            model.TransactionType = transactionTypesService.GetAll().Where(x => x.Parent == null && !x.IsHidden).First();
            return model;
        }


        public IEnumerable<ITransaction> GetAll(TransactionsFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            using (var context = GetContext())
            {
                StringBuilder query = new StringBuilder();
                query.Append(@"
SELECT 
[t].[TransactionId] AS [Id], 
[t].[Date] AS [Date], 
[t].[IncomeValue] AS [IncomeValue], 
[t].[ExpenseValue] AS [ExpenseValue], 
[t].[TransactionTypeId] AS [TransactionTypeId], 
[t].[TransactionKindId] AS [TransactionKindId], 
[t].[AccountFromId] AS [AccountFromId], 
[t].[AccountToId] AS [AccountToId], 
[t].[Comment] AS [Comment],
[t].[IsHidden] AS [IsHidden]
FROM [Transaction] AS [t]
WHERE ([t].[Date] >= @StartDate) AND ([t].[Date] <= @FinishDate) 
");
                if (!filter.ShowHidden)
                    query.Append(" AND IsHidden = 0");
                
                query.Append(" AND ( 1 = 0 ");
                foreach(var account in filter.Accounts)
                {
                    int accountId = ((AccountModel)account).Id;
                    query.AppendFormat(" OR [t].[AccountFromId] = {0} OR [t].[AccountToId] = {0}", accountId);
                }
                query.Append(" ) ");
                IEnumerable<Transaction> result = context.Database.SqlQuery<Transaction>(query.ToString(), 
                                new SqlCeParameter("@StartDate", filter.StartDate),
                                new SqlCeParameter("@FinishDate", filter.FinishDate));
                return result.ToList().Select(x => new TransactionModel(IoC, x));
            }
        }

        public void Save(ITransaction entity)
        {
            var dataItem = GetDataItem(entity);

            using (var context = GetContext())
            {
                if (dataItem.IsNew)
                    context.Set<Transaction>().Add(dataItem);
                else
                    context.AttachAsModified(dataItem);
                context.SaveChanges();
            }
        }

        public void Delete(ITransaction entity)
        {
            var dataItem = GetDataItem(entity);

            if (dataItem.IsNew)
                throw new ArgumentException("Item Transaction should be saved to database instance.", "item");

            using (var context = GetContext())
            {
                context.AttachAsModified(dataItem);
                context.Set<Transaction>().Remove(dataItem);
                context.SaveChanges();
            }
        }
    }
}
