using System.Runtime.Serialization;

// https://github.com/zkavtaskin/Domain-Driven-Design-Example

namespace Stock.Domain
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
