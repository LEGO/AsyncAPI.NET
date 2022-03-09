// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models.Bindings.OperationBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    internal class OperationBindingConverter : JsonDictionaryContractBindingConverter<IOperationBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IOperationBinding> value, JsonSerializer serializer)
        {
            this.PopulateInternal<IOperationBinding>(obj, value, serializer, new[]
            {
                new KeyValuePair<string, Type>("kafka", typeof(KafkaOperationBinding)),
                new KeyValuePair<string, Type>("http", typeof(HttpOperationBinding)),
            });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IOperationBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            this.WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}