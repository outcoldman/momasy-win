using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using outcold.MoMaSy.Data;

namespace MoMaSy.Tests
{
    public abstract class DatabaseTestBase : TestBase
    {
        private StorageContext _context;

        [TestInitialize]
        public void Initialize()
        {
            var storage = new Storage(IoC);
            storage.Init();
            _context = new StorageContext(storage);
            IoC.RegisterInstance(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}
