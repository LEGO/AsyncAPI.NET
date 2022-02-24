// Copyright (c) The LEGO Group. All rights reserved.

using Newtonsoft.Json.Serialization;

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models.Bindings.MessageBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class
        MessageJsonDictionaryContractBindingConverter : JsonDictionaryContractBindingConverter<IMessageBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IMessageBinding> value, JsonSerializer serializer)
        {
            this.populateInternal<IMessageBinding>(obj, value, serializer, new[] { new KeyValuePair<string, Type>("http", typeof(HttpMessageBinding)) });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IMessageBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            this.WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}