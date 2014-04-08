using System;

namespace outcold.MoMaSy.Data.DataModel
{
    internal class Transaction : Entity<Transaction>
    {
        public DateTime Date { get; set; }
        public decimal IncomeValue { get; set; }
        public decimal ExpenseValue { get; set; }

        public int TransactionTypeId { get; set; }
        public byte TransactionKindId { get; set; }

        public int? AccountFromId { get; set; }
        public int? AccountToId { get; set; }

        public string Comment { get; set; }

        public bool IsHidden { get; set; }
    }
}
