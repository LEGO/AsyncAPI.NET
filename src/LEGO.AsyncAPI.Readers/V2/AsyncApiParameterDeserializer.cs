// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiParameter> parameterFixedFields = new()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "schema", (a, n) => { a.Schema = JsonSchemaDeserializer.LoadSchema(n); } },
            { "location", (a, n) => { a.Location = n.GetScalarValue(); } },
        };

        private static PatternFieldMap<AsyncApiParameter> parameterPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiParameter LoadParameter(ParseNode node)
        {
            var mapNode = node.CheckMapNode("parameter");

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiParameter>(ReferenceType.Parameter, pointer);
            }

            var parameter = new AsyncApiParameter();

            ParseMap(mapNode, parameter, parameterFixedFields, parameterPatternFields);

            return parameter;
        }
    }
}