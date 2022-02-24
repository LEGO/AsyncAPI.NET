// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public abstract class BindingConverter<T, U> : JsonConverter where U : class
    {
        const string refProperty = "$ref";
        const string idProperty = "$id";

        protected virtual T Create(Type objectType, T existingValue, JsonSerializer serializer, JObject obj)
        {
            return existingValue ?? (T)serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
        }

        protected abstract void Populate(JObject obj, T value, JsonSerializer serializer);

        protected abstract void WriteProperties(JsonWriter writer, T value, JsonSerializer serializer, U contract);


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var contract = serializer.ContractResolver.ResolveContract(value.GetType());
            if (!(contract is U))
            {
                throw new JsonSerializationException($"Invalid non-object contract type {contract}");
            }

            if (value is not T)
            {
                throw new JsonSerializationException(
                    $"Converter cannot read JSON with the specified existing value. {typeof(T)} is required.");
            }

            writer.WriteStartObject();

            if (serializer.ReferenceResolver.IsReferenced(serializer, value))
            {
                writer.WritePropertyName(refProperty);
                writer.WriteValue(serializer.ReferenceResolver.GetReference(serializer, value));
            }
            else
            {
                writer.WritePropertyName(idProperty);
                writer.WriteValue(serializer.ReferenceResolver.GetReference(serializer, value));

                this.WriteProperties(writer, (T)value, serializer, contract as U);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var contract = serializer.ContractResolver.ResolveContract(objectType);
            if (contract is not U)
            {
                throw new JsonSerializationException($"Invalid non-object contract type {contract}");
            }

            if (existingValue is not(null or T))
            {
                throw new JsonSerializationException(
                    $"Converter cannot read JSON with the specified existing value. {typeof(T)} is required.");
            }

            if (JsonExtensions.MoveToContent(reader).TokenType == JsonToken.Null)
            {
                return null;
            }

            var obj = JObject.Load(reader);

            var refId = (string)obj[refProperty].RemoveFromLowestPossibleParent();
            var objId = (string)obj[idProperty].RemoveFromLowestPossibleParent();
            if (refId != null)
            {
                var reference = serializer.ReferenceResolver.ResolveReference(serializer, refId);
                if (reference != null)
                {
                    return reference;
                }
            }

            var value = this.Create(objectType, (T)existingValue, serializer, obj);

            if (objId != null)
            {
                // Add the empty array into the reference table BEFORE poppulating it, to handle recursive references.
                serializer.ReferenceResolver.AddReference(serializer, objId, value);
            }

            this.Populate(obj, value, serializer);

            return value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableTo(typeof(IDictionary<string, T>));
        }
    }
}