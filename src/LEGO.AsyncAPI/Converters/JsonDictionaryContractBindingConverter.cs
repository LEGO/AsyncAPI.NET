// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public abstract class JsonDictionaryContractBindingConverter<T> : BindingConverter<Dictionary<string, T>, JsonDictionaryContract>
    {
        protected void populateInternal<G>(JObject obj, IDictionary<string, T> value, JsonSerializer serializer, KeyValuePair<string, Type>[] typeMap)
            where G : T
        {
            foreach (var (key, type) in typeMap)
            {
                var token = obj[key];

                if (token is null)
                {
                    continue;
                }

                var binding = serializer.Deserialize(token.CreateReader(), type);
                value.Add(key, (G)binding);
            }
        }

        protected void WritePropertiesInternal(JsonWriter writer, IDictionary<string, T> value, JsonSerializer serializer, JsonDictionaryContract contract)
        {
            if (value is null)
            {
                return;
            }

            foreach (var (key, messageBinding) in value)
            {
                writer.WritePropertyName(key);
                serializer.Serialize(writer, messageBinding);
            }
        }
    }
}