using HigherOrLower.Utils.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HigherOrLower.Utils.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Converters = { new StringEnumConverter() },
            ContractResolver = new BaseFirstContractResolver()
        };

        public static string ToJsonMessage(this string message)
        {
            return ToJson(new { Message = message });
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, _serializerSettings);
        }
    }
}
