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
                    o.Headers = n.GetScalarValue();
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
                    o.CorrelationId = LoadRuntimeExpression(n)
                }
            },
            {
                "schemaFormat", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "contentType", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "name", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "title", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "description", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "tags", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "externalDocs", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            // { TODO
            //     "bindings", (o, n) =>
            //     {
            //         o.Url = n.GetScalarValue();
            //     }
            // },
            {
                "examples", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
            },
            {
                "traits", (o, n) =>
                {
                    o.Url = n.GetScalarValue();
                }
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