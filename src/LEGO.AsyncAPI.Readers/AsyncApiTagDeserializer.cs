using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiTag> _tagsFixedFields = new ()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "externalDocs", (a, n) => { a.ExternalDocs = LoadExternalDocs(n); } },
        };

        private static PatternFieldMap<AsyncApiTag> _tagsPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiTag LoadTag(ParseNode node)
        {
            var mapNode = node.CheckMapNode("tags");
            var tag = new AsyncApiTag();

            ParseMap(mapNode, tag, _tagsFixedFields, _tagsPatternFields);

            return tag;
        }
    }
}
