using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OblivionEngine.GameSystems;

[JsonConverter(typeof(StringEnumConverter))]
public enum LocationTypes
{
    [EnumMember(Value = "town")]
    Town,
    [EnumMember(Value = "city")]
    City,
    [EnumMember(Value = "transit")]
    Transit,
    [EnumMember(Value = "passage")]
    Passage,
    [EnumMember(Value = "keystone")]
    Keystone,
    [EnumMember(Value = "landmark")]
    Landmark
}