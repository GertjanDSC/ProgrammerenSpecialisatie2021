using System.Runtime.Serialization;

namespace Stock.Infrastructure.YahooFinanceApi
{
    public enum Period
    {
        [EnumMember(Value = "d")]
        Daily,

        [EnumMember(Value = "wk")]
        Weekly,

        [EnumMember(Value = "mo")]
        Monthly
    }
}
