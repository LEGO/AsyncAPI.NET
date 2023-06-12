// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiMessageExample> exampleFixedFields = new ()
        {
            { "headers", (a, n) => { a.Headers = n.CreateMap(LoadAny); } },
            { "payload", (a, n) => { a.Payload = n.CreateAny(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "summary", (a, n) => { a.Summary = n.GetScalarValue(); } },
        };

        private static PatternFieldMap<AsyncApiMessageExample> examplePatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiMessageExample LoadExample(ParseNode node)
        {
            var mapNode = node.CheckMapNode("example");
            var example = new AsyncApiMessageExample();

            ParseMap(mapNode, example, exampleFixedFields, examplePatternFields);

            return example;
        }
    }
}
