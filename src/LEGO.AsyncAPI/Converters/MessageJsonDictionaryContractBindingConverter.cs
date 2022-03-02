// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class MessageJsonDictionaryContractBindingConverter : JsonDictionaryContractBindingConverter<IMessageBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IMessageBinding> value, JsonSerializer serializer)
        {
            this.PopulateInternal<IMessageBinding>(obj, value, serializer, new[] { new KeyValuePair<string, Type>("http", typeof(HttpMessageBinding)) });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IMessageBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            this.WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}