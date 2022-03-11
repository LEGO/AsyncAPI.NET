// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    internal class ChannelJsonDictionaryContractBindingConverter : JsonDictionaryContractBindingConverter<IChannelBinding>
    {
        protected override void Populate(JObject obj, Dictionary<string, IChannelBinding> value, JsonSerializer serializer)
        {
            this.PopulateInternal<IChannelBinding>(obj, value, serializer, new[] { new KeyValuePair<string, Type>("kafka", typeof(KafkaChannelBinding)) });
        }

        protected override void WriteProperties(JsonWriter writer, Dictionary<string, IChannelBinding> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            this.WritePropertiesInternal(writer, value, serializer, contract);
        }
    }
}