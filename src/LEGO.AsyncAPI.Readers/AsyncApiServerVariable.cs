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
        private static readonly FixedFieldMap<AsyncApiServerVariable> _serverVariableFixedFields =
            new ()
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

        private static readonly PatternFieldMap<AsyncApiServerVariable> _serverVariablePatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiServerVariable LoadServerVariable(ParseNode node)
        {
            var mapNode = node.CheckMapNode("serverVariable");

            var serverVariable = new AsyncApiServerVariable();

            ParseMap(mapNode, serverVariable, _serverVariableFixedFields, _serverVariablePatternFields);

            return serverVariable;
        }
    }
}
