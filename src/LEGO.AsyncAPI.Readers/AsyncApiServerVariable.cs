// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiServerVariable> serverVariableFixedFields =
            new()
            {
                {
                    "enum", (a, n) => { a.Enum = n.CreateSimpleList(s => s.GetScalarValue()); }
                },
                {
                    "default", (a, n) => { a.Default = n.GetScalarValue(); }
                },
                {
                    "description", (a, n) => { a.Description = n.GetScalarValue(); }
                },
            };

        private static readonly PatternFieldMap<AsyncApiServerVariable> serverVariablePatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiServerVariable LoadServerVariable(ParseNode node)
        {
            var mapNode = node.CheckMapNode("serverVariable");

            var serverVariable = new AsyncApiServerVariable();

            ParseMap(mapNode, serverVariable, serverVariableFixedFields, serverVariablePatternFields);

            return serverVariable;
        }
    }
}
