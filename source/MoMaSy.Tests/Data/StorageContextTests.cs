using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using outcold.MoMaSy.Data;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Data.DataModel;

namespace MoMaSy.Tests.Data
{
    [TestClass]
    public class StorageContextTests : DatabaseTestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            var context = IoC.Resolve<StorageContext>();
            List<CurrencyType> list = context.CurrencyTypes.ToList();
            Assert.IsNotNull(list);

            List<Account> list1 = context.Accounts.ToList();
            Assert.IsNotNull(list1);

            List<TransactionType> list2 = context.TransactionTypes.ToList();
            Assert.IsNotNull(list2);

            List<Transaction> list3 = context.Transactions.ToList();
            Assert.IsNotNull(list3);
        }
    }
}
