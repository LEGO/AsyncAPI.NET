using LEGO.AsyncAPI.Any;
using Newtonsoft.Json;
using Array = LEGO.AsyncAPI.Any.Array;
using Boolean = LEGO.AsyncAPI.Any.Boolean;
using Double = LEGO.AsyncAPI.Any.Double;
using Object = LEGO.AsyncAPI.Any.Object;
using String = LEGO.AsyncAPI.Any.String;

namespace LEGO.AsyncAPI.Models
{
    public class PayloadConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            WriteValue(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return DocumentToAny(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAny);
        }

        private static void WriteValue(JsonWriter writer, object value)
        {
            switch (value)
            {
                case Object o:
                {
                    ObjectValue(writer, o);
                    break;
                }

                case Array a:
                {
                    ArrayValue(writer, a);
                    break;
                }

                case Double d:
                {
                    DoubleValue(writer, d);
                    break;
                }

                case String s:
                {
                    StringValue(writer, s);
                    break;
                }
                case Long l:
                {
                    LongValue(writer, l);
                    break;
                }
                case Boolean b:
                {
                    BooleanValue(writer, b);
                    break;
                }
                case Null:
                {
                    NullValue(writer);
                    break;
                }
                default:
                {
                    throw new JsonException();
                }
            }

            ;
        }

        private static void ObjectValue(JsonWriter writer, Object o)
        {
            writer.WriteStartObject();
            var enumerator = o.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var (key, value) = enumerator.Current;
                writer.WritePropertyName(key);
                WriteValue(writer, value);
            }

            writer.WriteEndObject();
        }


        private static void ArrayValue(JsonWriter writer, Array value)
        {
            writer.WriteStartArray();
            if (value.Value != null)
            {
                foreach (var item in value.Value)
                {
                    WriteValue(writer, item);
                }
            }

            writer.WriteEndArray();
        }

        private static void NullValue(JsonWriter writer)
        {
            writer.WriteNull();
        }

        private static void BooleanValue(JsonWriter writer, Boolean value)
        {
            writer.WriteValue(value.Value);
        }

        private static void LongValue(JsonWriter writer, Long value)
        {
            writer.WriteValue(value.Value);
        }

        private static void StringValue(JsonWriter writer, String value)
        {
            writer.WriteValue(value.Value);
        }

        private static void DoubleValue(JsonWriter writer, Double value)
        {
            writer.WriteValue(value.Value);
        }

        private IAny? DocumentToAny(JsonReader reader)
        {
            var readerTokenType = reader.TokenType;

            return readerTokenType switch
            {
                JsonToken.StartObject => DocumentToObject(reader),
                JsonToken.String => new String {Value = (string?) reader.Value},
                JsonToken.Float => new Double {Value = (double?) reader.Value},
                JsonToken.Integer => new Long {Value = (long) reader.Value},
                JsonToken.StartArray => DocumentToArray(reader),
                JsonToken.Boolean => new Boolean {Value = (bool?) reader.Value},
                JsonToken.Null => new Null(),
                _ => throw new JsonException("could not deserialize reader")
            };
        }

        private Object DocumentToObject(JsonReader asObject)
        {
            var result = new Object();

            while (asObject.Read())
            {
                if (asObject.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                var pathComponents = asObject.Path.Split('.');
                var path = pathComponents[^1];
                asObject.Read();
                var value = DocumentToAny(asObject);
                result.Add(path, value);
            }

            return result;
        }

        private Array DocumentToArray(JsonReader asObject)
        {
            var result = new Array();

            while (asObject.Read())
            {
                // Check for an empty array
                if (asObject.TokenType == JsonToken.EndArray)
                {
                    break;
                }

                result.Add(DocumentToAny(asObject));
            }

            return result;
        }
    }
}