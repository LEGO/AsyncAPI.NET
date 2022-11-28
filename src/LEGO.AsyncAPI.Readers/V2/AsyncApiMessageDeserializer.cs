// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;
    using System;

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
                "headers", (a, n) => { a.Headers = LoadSchema(n); }
            },
            {
                "payload", (a, n) => { a.Payload = LoadSchema(n); }
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

        private static SchemaFormat LoadSchemaFormat(string format)
        {
            if (format == string.Empty)
            {
                return SchemaFormat.AsyncApi;
            }

            var schemaFormat = format.GetEnumFromDisplayNameOrDefault<SchemaFormat>(SchemaFormat.Unsupported);
            if (schemaFormat == SchemaFormat.Unsupported)
            {
                throw new AsyncApiException($"SchemaFormat '{format}' is not supported");
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

            return message;
        }
    }
}