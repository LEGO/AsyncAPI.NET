using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class OperationBindingConverter : JsonDictionaryContractBindingConverter<IOperationBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IOperationBinding> value, JsonSerializer serializer)
        {
            populateInternal<IOperationBinding>(obj, value, serializer, new[]
            {
                new KeyValuePair<string, Type>("kafka", typeof(KafkaOperationBinding)),
                new KeyValuePair<string, Type>("http", typeof(HttpOperationBinding)),
            });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IOperationBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}