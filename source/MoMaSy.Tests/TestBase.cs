using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Settings;

namespace MoMaSy.Tests
{
    public abstract class TestBase
    {
        private IoC _ioc = new IoC();

        protected TestBase()
        {
            _ioc.RegisterInstance<ISettingsStore>(new SettingsStoreStub());
        }

        protected IoC IoC
        {
            get { return _ioc; }
        }
    }
}
