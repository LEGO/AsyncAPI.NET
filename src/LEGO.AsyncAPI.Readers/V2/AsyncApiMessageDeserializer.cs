// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiMessage> messageFixedFields = new()
        {
            {
                "messageId", (a, n) => { a.MessageId = n.GetScalarValue(); }
            },
            {
                "headers", (a, n) => { a.Headers = AsyncApiSchemaDeserializer.LoadSchema(n); }
            },
            {
                "payload", (a, n) => { a.Payload = null; /* resolved after the initial run */ }
            },
            {
                "correlationId", (a, n) => { a.CorrelationId = LoadCorrelationId(n); }
            },
            {
                "schemaFormat", (a, n) => { a.SchemaFormat = LoadSchemaFormat(n.GetScalarValue()); }
            },
            {
                "contentType", (a, n) => { a.ContentType = n.GetScalarValue(); }
            },
            {
                "name", (a, n) => { a.Name = n.GetScalarValue(); }
            },
            {
                "title", (a, n) => { a.Title = n.GetScalarValue(); }
            },
            {
                "summary", (a, n) => { a.Summary = n.GetScalarValue(); }
            },
            {
                "description", (a, n) => { a.Description = n.GetScalarValue(); }
            },
            {
                "tags", (a, n) => a.Tags = n.CreateList(LoadTag)
            },
            {
                "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); }
            },
            {
                "bindings", (a, n) => { a.Bindings = LoadMessageBindings(n); }
            },
            {
                "examples", (a, n) => a.Examples = n.CreateList(LoadExample)
            },
            {
                "traits", (a, n) => a.Traits = n.CreateList(LoadMessageTrait)
            },
        };

        public static IAsyncApiMessagePayload LoadJsonSchemaPayload(ParseNode n)
        {
            return LoadPayload(n, null);
        }

        public static IAsyncApiMessagePayload LoadAvroPayload(ParseNode n)
        {
            return LoadPayload(n, "application/vnd.apache.avro");
        }

        private static IAsyncApiMessagePayload LoadPayload(ParseNode n, string format)
        {
            if (n == null)
            {
                return null;
            }

            switch (format)
            {
                case null:
                case "":
                case var _ when SupportedJsonSchemaFormats.Where(s => format.StartsWith(s)).Any():
                    return new AsyncApiJsonSchemaPayload(AsyncApiSchemaDeserializer.LoadSchema(n));
                case var _ when SupportedAvroSchemaFormats.Where(s => format.StartsWith(s)).Any():
                    return new AsyncApiAvroSchemaPayload(AsyncApiAvroSchemaDeserializer.LoadSchema(n));
                default:
                    var supportedFormats = SupportedJsonSchemaFormats.Concat(SupportedAvroSchemaFormats);
                    throw new AsyncApiException($"'Could not deserialize Payload. Supported formats are {string.Join(", ", supportedFormats)}");
            }
        }

        static readonly IEnumerable<string> SupportedJsonSchemaFormats = new List<string>
        {
            "application/vnd.aai.asyncapi+json",
            "application/vnd.aai.asyncapi+yaml",
            "application/vnd.aai.asyncapi",
            "application/schema+json;version=draft-07",
            "application/schema+yaml;version=draft-07",
        };

        static readonly IEnumerable<string> SupportedAvroSchemaFormats = new List<string>
        {
            "application/vnd.apache.avro+json;version=1.9.0",
            "application/vnd.apache.avro+yaml;version=1.9.0",
        };

        private static string LoadSchemaFormat(string schemaFormat)
        {
            var supportedFormats = SupportedJsonSchemaFormats.Concat(SupportedAvroSchemaFormats);
            if (!supportedFormats.Where(s => schemaFormat.StartsWith(s)).Any())
            {
                throw new AsyncApiException($"'{schemaFormat}' is not a supported format. Supported formats are {string.Join(", ", supportedFormats)}");
            }

            return schemaFormat;
        }

        private static readonly PatternFieldMap<AsyncApiMessage> messagePatternFields = new()
        {
            { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
        };

        public static AsyncApiMessage LoadMessage(ParseNode node)
        {
            var mapNode = node.CheckMapNode("message");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiMessage>(ReferenceType.Message, pointer);
            }

            var message = new AsyncApiMessage();

            ParseMap(mapNode, message, messageFixedFields, messagePatternFields);
            message.Payload = LoadPayload(mapNode["payload"]?.Value, message.SchemaFormat);

            return message;
        }
    }
}