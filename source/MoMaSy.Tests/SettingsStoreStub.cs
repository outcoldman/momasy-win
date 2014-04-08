using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using outcold.MoMaSy.Core.Settings;

namespace MoMaSy.Tests
{
    class SettingsStoreStub : ISettingsStore
    {
        public object Get(string index)
        {
            throw new NotImplementedException();
        }

        public object Get(string index, object defaultValue)
        {
            return defaultValue;
        }

        public void Set(string index, object value)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
