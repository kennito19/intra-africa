using Newtonsoft.Json;

namespace API_Gateway.Helper
{
    public class CaseInsensitiveDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, decimal>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dictionary = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            serializer.Populate(reader, dictionary);
            return dictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
