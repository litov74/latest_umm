using System.Text.Json.Serialization;

namespace API.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanySubscriptionPlanEnum
    {
        Trial = 1,
        Basic = 2,
        Businness = 3,
        Enterprise = 4
    }
}
