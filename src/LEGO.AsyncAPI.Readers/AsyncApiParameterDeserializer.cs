using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiParameter> _parameterFixedFields = new ()
        {
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "schema", (a, n) => { a.Schema = LoadSchema(n); } },
            { "location", (a, n) => { a.Location = n.GetScalarValue(); } },
        };
        
        private static PatternFieldMap<AsyncApiParameter> _parameterPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
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

            ParseMap(mapNode, parameter, _parameterFixedFields, _parameterPatternFields);

            return parameter;
        }
    }
}
