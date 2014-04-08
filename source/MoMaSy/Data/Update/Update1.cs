using System;
using System.Data.SqlServerCe;
using System.Linq;
using outcold.MoMaSy.Model;

namespace outcold.MoMaSy.Data.Update
{
    class Update1 : UpdateBase
    {
        public override int GetUpdateVersion()
        {
            return 1;
        }

        public override bool Update(Storage storage)
        {
            using (SqlCeConnection connection = storage.GetConnection())
            {
                connection.Open();
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
CREATE TABLE [CurrencyType] ( 
    CurrencyTypeId int identity(1,1) NOT NULL PRIMARY KEY, 
    CurrencyName nvarchar(3) NOT NULL,
    IsHidden bit NOT NULL default(0)
);";
                    command.ExecuteNonQuery();
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
CREATE TABLE [Account] ( 
    AccountId int identity(1,1) NOT NULL PRIMARY KEY, 
    AccountName nvarchar(30) NOT NULL, 
    CurrencyTypeId int,
    IsHidden bit NOT NULL default(0),
    CONSTRAINT FK_Account_CurrencyType FOREIGN KEY (CurrencyTypeId) REFERENCES CurrencyType (CurrencyTypeId)
);";
                    command.ExecuteNonQuery();
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
CREATE TABLE [TransactionKind] ( 
    TransactionKindId tinyint NOT NULL PRIMARY KEY, 
    TransactionKindName nvarchar(8) NOT NULL
);
";
                    command.ExecuteNonQuery();
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
CREATE TABLE [TransactionType] ( 
    TransactionTypeId int identity(1,1) NOT NULL PRIMARY KEY, 
    TransactionTypeName nvarchar(50) NOT NULL, 
    IsSystem bit NOT NULL default(0), 
    TransactionParentTypeId int NULL,
    IsHidden bit NOT NULL default(0),
    CONSTRAINT FK_TransactionType_TransactionType_Parent FOREIGN KEY (TransactionParentTypeId) REFERENCES TransactionType (TransactionTypeId)
);";
                    command.ExecuteNonQuery();
                }

                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
CREATE TABLE [Transaction] ( 
    TransactionId int identity(1,1) NOT NULL PRIMARY KEY, 
    Date datetime NOT NULL, 
    IncomeValue money NOT NULL default(0), 
    ExpenseValue money NOT NULL default(0), 
    TransactionTypeId int NOT NULL, 
    TransactionKindId tinyint NOT NULL,
    AccountFromId int NULL,
    AccountToId int NULL,
    Comment nvarchar(512) NULL,
    IsHidden bit NOT NULL default(0),
    CONSTRAINT FK_Transaction_TransactionType FOREIGN KEY (TransactionTypeId) REFERENCES TransactionType (TransactionTypeId),
    CONSTRAINT FK_Transaction_TransactionKind FOREIGN KEY (TransactionKindId) REFERENCES TransactionKind (TransactionKindId),
    CONSTRAINT FK_Transaction_Account_From FOREIGN KEY (AccountFromId) REFERENCES Account (AccountId),
    CONSTRAINT FK_Transaction_Account_To FOREIGN KEY (AccountToId) REFERENCES Account (AccountId)
);";
                    command.ExecuteNonQuery();
                }

                foreach (TransactionKind kind in Enum.GetValues(typeof(TransactionKind)).Cast<TransactionKind>())
                {
                    using (SqlCeCommand command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format(@"
INSERT INTO [TransactionKind] (TransactionKindId, TransactionKindName) VALUES ({0:D}, '{0:G}');", kind);
                        command.ExecuteNonQuery();
                    }
                }
            }

            return true;
        }
    }
}
