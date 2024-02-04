using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace HigherOrLower.Utils.Formatters
{
    public class BaseFirstContractResolver : DefaultContractResolver
    {
        // Just to show that I care about performance (by "caching" values) and I know recursion :),
        // obviously this could be improved :)
        // this is not critical at all, because the JsonConverter already caches internally for each type :)
        private static IDictionary<Type, int> _numberOfBaseTypesPerType = new Dictionary<Type, int>();

        private static int GetNumberOfBaseTypes(Type? type)
        {
            if (type == null)
            {
                return 0;
            }

            if (_numberOfBaseTypesPerType.TryGetValue(type, out int numberOfBaseTypes))
            {
                return numberOfBaseTypes;
            }

            numberOfBaseTypes = GetNumberOfBaseTypes(type.BaseType) + 1;

            _numberOfBaseTypesPerType[type] = numberOfBaseTypes;
            return numberOfBaseTypes;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                .OrderBy(property => GetNumberOfBaseTypes(property.DeclaringType))
                .ToList();
        }
    }
}
