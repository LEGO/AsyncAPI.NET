using JetBrains.Annotations;
using Newtonsoft.Json;

namespace LEGO.AsyncAPI.Models.ChannelBindings;

public class ChannelBindingConverter: JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var workingCopy = value as Dictionary<string, IChannelBinding>;
        writer.WriteStartObject();
        foreach (var channelBinding in workingCopy)
        {
            writer.WritePropertyName(channelBinding.Key);
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return DocumentToChannelBinding(reader);
    }

    [CanBeNull]
    private IDictionary<string, IChannelBinding> DocumentToChannelBinding(JsonReader reader)
    {
        var readerTokenType = reader.TokenType;

        return readerTokenType switch
        {
            JsonToken.StartObject => DocumentToObject(reader),
            JsonToken.Null => null,
            _ => throw new JsonException("could not deserialize reader")
        };
    }

    private IDictionary<string, IChannelBinding> DocumentToObject(JsonReader asObject)
    {
        var result = new Dictionary<string, IChannelBinding>();

        while (asObject.Read())
        {
            if (DetectObjectEndAndObjectReferences(asObject))
            {
                break;
            }

            string[] pathComponents;
            string path;

            pathComponents = asObject.Path.Split('.');
            path = pathComponents[^1];

            switch (path)
            {
                case "kafka":
                {
                    result.Add(path, ValueToKafkaBinding(asObject));
                    break;
                }

                default:
                {
                    throw new JsonException("Channel Binding type not supported: " + path + " Supported types: Kafka");
                }
            }
        }

        return result;
    }

    private static bool DetectObjectEndAndObjectReferences(JsonReader asObject)
    {
        if (asObject.TokenType == JsonToken.EndObject)
        {
            return true;
        }

        var pathComponents = asObject.Path.Split('.');
        var path = pathComponents[^1];

        // reference $id is not handle for the binding converter yet
        if (path == "$id")
        {
            throw new JsonException("Sorry, the $id property is not supported for channel bindings yet");
        }
        else
        {
            asObject.Read();
        }

        if (asObject.TokenType == JsonToken.EndObject)
        {
            return true;
        }

        return false;
    }

    private IChannelBinding ValueToKafkaBinding(JsonReader asObject)
    {
        var result = new KafkaChannelBinding();

        while (asObject.Read())
        {
            // Note: The spec only reserves the Kafka channel binding. It does not support any properties
            if (asObject.TokenType == JsonToken.EndObject)
            {
                break;
            }
        }

        return result;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IDictionary<string, IChannelBinding>);
    }
}