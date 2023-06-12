// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiExternalDocumentation> externalDocumentationFixedFields = new ()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "url", (a, n) => { a.Url = new Uri(n.GetScalarValue()); } },
        };

        private static PatternFieldMap<AsyncApiExternalDocumentation> externalDocumentationPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiExternalDocumentation LoadExternalDocs(ParseNode node)
        {
            var mapNode = node.CheckMapNode("externalDocs");
            var components = new AsyncApiExternalDocumentation();

            ParseMap(mapNode, components, externalDocumentationFixedFields, externalDocumentationPatternFields);

            return components;
        }
    }
}
