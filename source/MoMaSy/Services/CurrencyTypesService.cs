using System;
using System.Linq;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Data.DataModel;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;

namespace outcold.MoMaSy.Services
{
    internal interface ICurrencyTypesService : IDictionaryEntityService<ICurrencyType>
    {
        ICurrencyType CreateNew();
        bool CanBeDeleted(ICurrencyType currencyType, out string errorMessage);
    }

    internal class CurrencyTypesService : DictionaryServiceBase<ICurrencyType, CurrencyType>, ICurrencyTypesService
    {
        public CurrencyTypesService(IoC ioc)
            : base(ioc)
        {

        }

        protected override ICurrencyType CreateContainer(CurrencyType entity)
        {
            return new CurrencyTypeModel(entity);
        }

        ICurrencyType ICurrencyTypesService.CreateNew()
        {
            var currencyType = new CurrencyType()
            {
                CurrencyName = StringResources.CurrencyType_NewName
            };

            var result = CreateContainer(currencyType);
            ((ICurrencyTypesService)this).Save(result);
            return result;
        }

        bool ICurrencyTypesService.CanBeDeleted(ICurrencyType currencyType, out string errorMessage)
        {
            var dataItem = GetDataItem(currencyType);
            if (dataItem.IsNew)
                throw new ArgumentException("Only saved currency type can be verified", "currencyType");

            errorMessage = null;

            using (var context = GetContext())
            {
                if (context.Accounts.Any(x => (x.CurrencyTypeId == dataItem.Id)))
                    errorMessage = StringResources.ErrMsg_CurrencyType_UsedByAccount;
            }

            return errorMessage == null;
        }
    }
}
