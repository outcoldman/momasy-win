using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Reflection;

namespace outcold.MoMaSy.Data.Update
{
    internal class UpdateManager
    {
        private Storage _storage;

        private DataTable _tablesSchema;

        public UpdateManager(Storage storage)
        {
            _storage = storage;
        }

        public void Update()
        {
            LoadInfo();
            if (!_tablesSchema.Rows.Cast<DataRow>().Any(x => string.Compare(x.Field<string>("TABLE_NAME"), "System", true) == 0))
            {
                CreateVersionTable();
            }

            int currentVersion = GetVersion();

            foreach (UpdateBase update in GetUpdates())
            {
                int updateVersion = update.GetUpdateVersion();

                if (update.GetUpdateVersion() <= currentVersion)
                    continue;

                if (!update.Update(_storage))
                    throw new StorageException(string.Format("Can't update to version '{0}'.", updateVersion));

                UpdateVersion(updateVersion);

                currentVersion = updateVersion;
            }
        }

        private void LoadInfo()
        {
            using (SqlCeConnection connection = _storage.GetConnection())
            {
                connection.Open();
                _tablesSchema = connection.GetSchema("Tables");
            }
        }

        private void CreateVersionTable()
        {
            using (SqlCeConnection connection = _storage.GetConnection())
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE [System] ( VersionId int NOT NULL );";
                    command.ExecuteNonQuery();
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [System] ( VersionId ) values ( 0 )";
                    command.ExecuteNonQuery();
                }
            }
        }

        private int GetVersion()
        {
            using (SqlCeConnection connection = _storage.GetConnection())
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT VersionId FROM [System]";
                    object obj = command.ExecuteScalar();

                    if (obj == null)
                        throw new StorageException("Couldn't get version from database.");

                    return int.Parse(obj.ToString());
                }
            }
        }

        private void UpdateVersion(int version)
        {
            using (SqlCeConnection connection = _storage.GetConnection())
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [System] SET VersionId = @VersionId";
                    command.Parameters.AddWithValue("@VersionId", version);
                    int result = command.ExecuteNonQuery();
                    if (result != 1)
                        throw new StorageException("Only one row could be in Version Table.");
                }
            }
        }

        private IEnumerable<UpdateBase> GetUpdates()
        {
            Type currentType = GetType();
            Assembly assembly = GetType().Assembly;
            return assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(UpdateBase)))
                .Select(type => (UpdateBase)Activator.CreateInstance(type))
                .OrderBy(x => x.GetUpdateVersion()).AsEnumerable();
        }
    }
}
