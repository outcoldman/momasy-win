namespace outcold.MoMaSy.Data.DataModel
{
    internal class Account : Entity<Account>
    {
        public string AccountName { get; set; }
        public int CurrencyTypeId { get; set; }
        public bool IsHidden { get; set; }
    }
}
