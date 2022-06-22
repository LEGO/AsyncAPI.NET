using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiExternalDocumentation> _externalDocumentationFixedFields = new ()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "url", (a, n) => { a.Url = new Uri(n.GetScalarValue()); } },
        };

        private static PatternFieldMap<AsyncApiExternalDocumentation> _externalDocumentationPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiExternalDocumentation LoadExternalDocs(ParseNode node)
        {
            var mapNode = node.CheckMapNode("externalDocs");
            var components = new AsyncApiExternalDocumentation();

            ParseMap(mapNode, components, _externalDocumentationFixedFields, _externalDocumentationPatternFields);

            return components;
        }
    }
}
