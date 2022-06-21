using LEGO.AsyncAPI.Models;
using LEGO.AsyncApi.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiComponents> _componentsFixedFields = new FixedFieldMap<AsyncApiComponents>
        {
            {"schemas", (a, n) => a.Schemas = n.CreateMapWithReference(ReferenceType.Schema, LoadSchema)},
            {"servers", (a, n) => a.Servers = n.CreateMapWithReference(ReferenceType.Server, )},
            {"channels", (a, n) => a.Channels = n.CreateMapWithReference(ReferenceType.Channel, )},
            {"messages", (a, n) => a.Messages = n.CreateMapWithReference(ReferenceType.Message, )},
            {"securitySchemas", (a, n) => a.SecuritySchemes = n.CreateMapWithReference(ReferenceType.SecurityScheme, )},
        };


        private static PatternFieldMap<AsyncApiComponents> _componentsPatternFields =
            new()
            {
                {s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n))}
            };

        public static AsyncApiComponents LoadComponents(ParseNode node)
        {
            var mapNode = node.CheckMapNode("components");
            var components = new AsyncApiComponents();

            ParseMap(mapNode, components, _componentsFixedFields, _componentsPatternFields);

            return components;
        }
    }
}