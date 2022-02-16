using LEGO.AsyncAPI.NewtonUtils;

namespace LEGO.AsyncAPI
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public abstract class BindingConverter<T> : JsonConverter
    where T : IBinding
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ObjectToJToken.Map(value).WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var readFrom = JToken.ReadFrom(reader);
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var (key, type) in GetBindingTypeMap())
            {
                var jToken = readFrom[key];
                if (jToken == null)
                {
                    continue;
                }

                result.Add(key, ObjectFromJToken.Map(type, jToken));
            }

            return result
                .ToDictionary(k => k.Key, v => (T)v.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableTo(typeof(IDictionary<string, T>));
        }

        protected abstract ImmutableDictionary<string, Type> GetBindingTypeMap();
    }
}