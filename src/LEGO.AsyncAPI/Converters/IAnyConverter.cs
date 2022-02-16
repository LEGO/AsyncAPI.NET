using LEGO.AsyncAPI.NewtonUtils;

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Any;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class IAnyConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IAnyToJToken.Map(value as IAny).WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IAnyFromJToken.Map(JToken.Load(reader));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAny);
        }
    }
}