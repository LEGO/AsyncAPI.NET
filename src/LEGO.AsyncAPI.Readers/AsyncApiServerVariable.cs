using LEGO.AsyncApi.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncApi.Readers.ParseNodes;

namespace LEGO.AsyncApi.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiServerVariable> _serverVariableFixedFields =
            new FixedFieldMap<AsyncApiServerVariable>
            {
                {
                    "enum", (o, n) =>
                    {
                        o.Enum = n.CreateSimpleList(s => s.GetScalarValue());
                    }
                },
                {
                    "default", (o, n) =>
                    {
                        o.Default = n.GetScalarValue();
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
            };

        private static readonly PatternFieldMap<AsyncApiServerVariable> _serverVariablePatternFields =
            new PatternFieldMap<AsyncApiServerVariable>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
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