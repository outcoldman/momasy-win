using Microsoft.VisualStudio.TestTools.UnitTesting;
using outcold.MoMaSy.Services;

namespace MoMaSy.Tests.Services
{
    [TestClass]
    public class CurrencyTypesServiceTests : DatabaseTestBase
    {
        [TestMethod]
        public void GetCurrencyTypes_ShouldReturnCopy()
        {
            ICurrencyTypesService service = IoC.Resolve<CurrencyTypesService>();
            var list1 = service.GetAll();
            var list2 = service.GetAll();

            Assert.AreNotSame(list1, list2);
        }
    }
}
