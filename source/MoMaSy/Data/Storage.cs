using System;
using System.Data.SqlServerCe;
using System.IO;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Settings;
using outcold.MoMaSy.Data.Update;

namespace outcold.MoMaSy.Data
{
    internal class Storage
    {
        private string _dataFilePath;
        private string _connectionString;
        private bool _isInitialized = false;
        private IoC _ioc;

        public Storage(IoC ioc)
        {
            _ioc = ioc;
            FindDataFile();
        }
        
        public void Init()
        {
            if (_isInitialized)
                throw new InvalidOperationException("Storage already initialized");

            using (SqlCeEngine engine = new SqlCeEngine(ConnectionString))
            {
              if (!File.Exists(_dataFilePath))
                    engine.CreateDatabase();
            }

            _isInitialized = true;

            UpdateManager updateManager = new UpdateManager(this);
            updateManager.Update();
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public SqlCeConnection GetConnection()
        {
            if (!_isInitialized)
                throw new NotSupportedException("Object should be initialized first.");

            return new SqlCeConnection(ConnectionString);
        }

        private void FindDataFile()
        {
            ISettingsStore settings = _ioc.Resolve<ISettingsStore>();
            _dataFilePath = settings.Get("DataFilePath", @"Data\Data.sdf").ToString();
            _dataFilePath = Path.GetFullPath(_dataFilePath);
            string directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            _connectionString = string.Format("Data Source = {0}", _dataFilePath);
        }
    }
}
