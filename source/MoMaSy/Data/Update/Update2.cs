using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace outcold.MoMaSy.Data.Update
{
    class Update2 : UpdateBase
    {
        public override int GetUpdateVersion()
        {
            return 2;
        }

        public override bool Update(Storage storage)
        {
            using (SqlCeConnection connection = storage.GetConnection())
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
INSERT INTO [TransactionType] (TransactionTypeName, IsSystem, TransactionParentTypeId, IsHidden) VALUES ('Transfer', 1, NULL, 0);";
                    command.ExecuteNonQuery();
                }

                int transactionTypeId;
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT TransactionTypeID FROM TransactionType WHERE IsSystem = 1";
                    var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        Debug.Assert(false, "!reader.Read()");
                        return false;
                    }
                    transactionTypeId = reader.GetInt32(ordinal: 0);
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
UPDATE [Transaction] SET [TransactionTypeID] = @transactionTypeId
WHERE AccountFromId IS NOT NULL AND AccountToId IS NOT NULL";
                    command.Parameters.AddWithValue("@transactionTypeId", transactionTypeId);
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
