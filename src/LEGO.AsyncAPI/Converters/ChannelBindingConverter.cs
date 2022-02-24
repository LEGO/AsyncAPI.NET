// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class ChannelJsonDictionaryContractBindingConverter : JsonDictionaryContractBindingConverter<IChannelBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IChannelBinding> value, JsonSerializer serializer)
        {
            this.populateInternal<IChannelBinding>(obj, value, serializer, new[] { new KeyValuePair<string, Type>("kafka", typeof(KafkaChannelBinding)) });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IChannelBinding> value,
            JsonSerializer serializer, JsonDictionaryContract contract)
        {
            this.WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}