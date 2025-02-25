using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OblivionEngine.GameSystems;

[JsonConverter(typeof(StringEnumConverter))]
public enum BuildingTypes
{
    [EnumMember(Value = "house")]
    House
}