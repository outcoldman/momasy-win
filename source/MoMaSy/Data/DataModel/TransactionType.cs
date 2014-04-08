namespace outcold.MoMaSy.Data.DataModel
{
    internal class TransactionType : Entity<TransactionType>
    {
        public string TransactionTypeName { get; set; }
        public bool IsSystem { get; set; }
        public int? TransactionParentTypeId { get; set; }
        public bool IsHidden { get; set; }
    }
}
