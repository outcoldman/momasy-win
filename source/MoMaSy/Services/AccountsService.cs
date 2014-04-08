using System;
using System.Linq;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.Services
{
    internal interface IAccountsService : IDictionaryEntityService<IAccount>
    {
        bool CanCreateNew(out string errorMessage);
        IAccount CreateNew();
        bool CanBeDeleted(IAccount account, out string errorMessage);
        decimal GetAccountAmount(IAccount account);
    }

    internal class AccountsService : DictionaryServiceBase<IAccount, Account>, IAccountsService
    {
        public AccountsService(IoC ioc)
            : base(ioc)
        {

        }

        protected override IAccount CreateContainer(Account entity)
        {
            return new AccountModel(IoC, entity);
        }

        bool IAccountsService.CanCreateNew(out string errorMessage)
        {
            errorMessage = null;

            var currencyTypesService = IoC.Resolve<ICurrencyTypesService>();

            if (!currencyTypesService.GetAll().Any(x => !x.IsHidden))
                errorMessage = StringResources.ErrMsg_Account_CannotCreateNew;

            return errorMessage == null;
        }

        IAccount IAccountsService.CreateNew()
        {
#if DEBUG
            string errorMessage;
            if (!((IAccountsService)this).CanCreateNew(out errorMessage))
                throw new InvalidOperationException();
#endif
            var currencyTypesService = IoC.Resolve<ICurrencyTypesService>();
            var currencyType = currencyTypesService.GetAll().Where(x => !x.IsHidden).First();

            var acccount = new Account()
            {
                AccountName = StringResources.Account_NewName,
            };

            var result = CreateContainer(acccount);
            
            result.CurrencyType = currencyType;

            ((IAccountsService)this).Save(result);
            return result;
        }

        bool IAccountsService.CanBeDeleted(IAccount account, out string errorMessage)
        {
            var dataItem = GetDataItem(account);

            if (dataItem.IsNew)
                throw new ArgumentException("Only saved account can be verified", "account");

            errorMessage = null;

            using (var context = GetContext())
            {
                if (context.Transactions.Any(x => (x.AccountFromId.HasValue && x.AccountFromId.Value == dataItem.Id)
                        || (x.AccountToId.HasValue && x.AccountToId.Value == dataItem.Id)))
                    errorMessage = StringResources.ErrMsg_Account_UsedByTransaction;
            }

            return errorMessage == null;
        }

        public decimal GetAccountAmount(IAccount account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            var dataItem = GetDataItem(account);

            decimal income;
            using (var context = GetContext())
            {
                income = context.Transactions.Where(x => x.IsHidden == false && x.AccountToId.HasValue && x.AccountToId.Value == dataItem.Id)
                    .Sum(x => (decimal?)x.IncomeValue) ?? 0;
            }

            decimal expense;
            using (var context = GetContext())
            {
                expense = context.Transactions.Where(x => x.IsHidden == false && x.AccountFromId.HasValue && x.AccountFromId.Value == dataItem.Id)
                    .Sum(x => (decimal?)x.ExpenseValue) ?? 0;
            }

            return income - expense;
        }
    }
}
