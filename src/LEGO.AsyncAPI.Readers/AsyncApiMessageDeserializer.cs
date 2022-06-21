using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiMessage> _messageFixedFields = new FixedFieldMap<AsyncApiMessage>
        {
            {
                "headers", (o, n) =>
                {
                    o.Headers = LoadSchema(n);
                }
            },
            {
                "payload", (o, n) =>
                {
                    o.Payload = n.CreateAny();
                }
            },
            {
                "correlationId", (o, n) =>
                {
                    o.CorrelationId = LoadCorrelationId(n);
                }
            },
            {
                "schemaFormat", (o, n) =>
                {
                    o.SchemaFormat = n.GetScalarValue();
                }
            },
            {
                "contentType", (o, n) =>
                {
                    o.ContentType = n.GetScalarValue();
                }
            },
            {
                "name", (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                "title", (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Description = n.GetScalarValue();
                }
            },
            {
                "tags", (a, n) => a.Tags = n.CreateList(LoadTag)
            },
            {
                "externalDocs", (o, n) =>
                {
                    o.ExternalDocs = LoadExternalDocs(n);
                }
            },
            // { TODO
            //     "bindings", (o, n) =>
            //     {
            //         o.Url = n.GetScalarValue();
            //     }
            // },
            {
                "examples", (a, n) => a.Examples = n.CreateList(LoadExample)
            },
            {
                "traits", (a, n) => a.Traits = n.CreateList(LoadTrait)
            },
        };

        private static readonly PatternFieldMap<AsyncApiMessage> _messagePatternFields = new PatternFieldMap<AsyncApiMessage>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiMessage LoadMessage(ParseNode node)
        {
            var mapNode = node.CheckMapNode("message");

            var message = new AsyncApiMessage();

            ParseMap(mapNode, message, _messageFixedFields, _messagePatternFields);

            return message;
        }
    }
}