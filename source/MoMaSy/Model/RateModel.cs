using outcold.MoMaSy.Data.DataModel;

namespace outcold.MoMaSy.Model
{
    internal interface IRate : ICurrencyType
    {
        decimal CurrencyRate { get; set; }
    }

    internal class RateModel : CurrencyTypeModel, IRate
    {
        public RateModel()
            : base(new CurrencyType())
        {

        }

        public decimal CurrencyRate { get; set; }
    }
}
