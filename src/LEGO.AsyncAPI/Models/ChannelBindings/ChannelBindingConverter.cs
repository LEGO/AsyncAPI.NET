using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models.ChannelBindings;

public class ChannelBindingConverter: JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ChannelBindingToJToken.Map(value as IDictionary<string, IChannelBinding>).WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return ChannelBindingFromJToken.Map(JToken.ReadFrom(reader));
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IDictionary<string, IChannelBinding>);
    }
}