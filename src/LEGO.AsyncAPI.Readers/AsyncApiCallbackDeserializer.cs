using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi  document into
    /// runtime Open API object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiCallback> _callbackFixedFields =
            new FixedFieldMap<AsyncApiCallback>();

        private static readonly PatternFieldMap<AsyncApiCallback> _callbackPatternFields =
            new PatternFieldMap<AsyncApiCallback>
            {
                {s => !s.StartsWith("x-"), (o, p, n) => o.AddPathItem(RuntimeExpression.Build(p), LoadPathItem(n))},
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))},
            };

        public static AsyncApiCallback LoadCallback(ParseNode node)
        {
            var mapNode = node.CheckMapNode("callback");

            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiCallback>(ReferenceType.Callback, pointer);
            }

            var domainObject = new AsyncApiCallback();

            ParseMap(mapNode, domainObject, _callbackFixedFields, _callbackPatternFields);

            return domainObject;
        }
    }
}