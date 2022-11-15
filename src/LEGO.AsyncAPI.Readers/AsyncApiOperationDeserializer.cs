// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiOperation> operationFixedFields =
            new()
            {
                {
                    "operationId", (a, n) => { a.OperationId = n.GetScalarValue(); }
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
                    "bindings", (a, n) => { a.Bindings = LoadOperationBindings(n); }
                },
                {
                    "traits", (a, n) => { a.Traits = n.CreateList(LoadOperationTrait); }
                },
                {
                    "message", (a, n) => { a.Message = LoadMessages(n); }
                },
            };

        private static IList<AsyncApiMessage> LoadMessages(ParseNode n)
        {
            var mapNode = n.CheckMapNode("message");

            if (mapNode["oneOf"] != null)
            {
                return mapNode["oneOf"].Value.CreateList(LoadMessage);
            }

            return new List<AsyncApiMessage> { LoadMessage(n) };
        }

        private static readonly PatternFieldMap<AsyncApiOperation> operationPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n)) },
            };

        internal static AsyncApiOperation LoadOperation(ParseNode node)
        {
            var mapNode = node.CheckMapNode("operation");

            var operation = new AsyncApiOperation();

            ParseMap(mapNode, operation, operationFixedFields, operationPatternFields);

            return operation;
        }
    }
}