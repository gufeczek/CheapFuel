using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebAPI.Common.Json;

public class BaseFieldsFirstContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        return base.CreateProperties(type, memberSerialization)
            .OrderBy(p => p.DeclaringType?.BaseTypeAndSelf().Count()).ToList();
    }
}

public static class TypeExtensions
{
    public static IEnumerable<Type> BaseTypeAndSelf(this Type type)
    {
        while (type != null)
        {
            yield return type;
            type = type.BaseType;
        }
    }
}