namespace outcold.MoMaSy.Data.DataModel
{
    internal class CurrencyType : Entity<CurrencyType>
    {
        public string CurrencyName { get; set; }
        public bool IsHidden { get; set; }
    }
}
