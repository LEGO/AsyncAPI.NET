namespace LEGO.AsyncAPI.Converters
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models.Bindings.ServerBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class ServerBindingConverter : JsonDictionaryContractBindingConverter<IServerBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IServerBinding> value, JsonSerializer serializer)
        {
            populateInternal<IServerBinding>(obj, value, serializer, new[] { new KeyValuePair<string, Type>("kafka", typeof(KafkaServerBinding)) });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IServerBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}