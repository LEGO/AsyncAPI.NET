// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiMessageTrait> messageTraitFixedFields = new()
        {
            { "messageId", (a, n) => { a.MessageId = n.GetScalarValue(); } },
            { "headers", (a, n) => { a.Headers = LoadSchema(n); } },
            { "correlationId", (a, n) => { a.CorrelationId = LoadCorrelationId(n); } },
            { "schemaFormat", (a, n) => { a.SchemaFormat = n.GetScalarValue(); } },
            { "contentType", (a, n) => { a.ContentType = n.GetScalarValue(); } },
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "title", (a, n) => { a.Title = n.GetScalarValue(); } },
            { "summary", (a, n) => { a.Summary = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "tags", (a, n) => { a.Tags = n.CreateList(LoadTag); } },
            { "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); } },
            { "bindings", (a, n) => { a.Bindings = LoadMessageBindings(n); } },
            { "examples", (a, n) => { a.Examples = n.CreateList(LoadExample); } },
        };

        private static PatternFieldMap<AsyncApiMessageTrait> messageTraitPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiMessageTrait LoadMessageTrait(ParseNode node)
        {
            var mapNode = node.CheckMapNode("traits");
            var pointer = mapNode.GetReferencePointer();

            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiMessageTrait>(ReferenceType.MessageTrait, pointer);
            }

            var messageTrait = new AsyncApiMessageTrait();

            ParseMap(mapNode, messageTrait, messageTraitFixedFields, messageTraitPatternFields);
            return messageTrait;
        }
    }
}
