using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OblivionEngine.GameSystems;

[JsonConverter(typeof(StringEnumConverter))]
public enum LocationZones
{
    [EnumMember(Value = "AurelianZone")]
    Aurelian,
    [EnumMember(Value = "MountAethoriaZone")]
    MountAethoria,
    [EnumMember(Value = "OpeningZone")]
    Opening,
    [EnumMember(Value = "ScorchedZone")]
    Scorched,
    [EnumMember(Value = "SubmergedZone")]
    Submerged,
    [EnumMember(Value = "TidecliffZone")]
    Tidecliff,
    [EnumMember(Value = "VoltwoodZone")]
    Voltwood,
    [EnumMember(Value = "ZenithZone")]
    Zenith
}