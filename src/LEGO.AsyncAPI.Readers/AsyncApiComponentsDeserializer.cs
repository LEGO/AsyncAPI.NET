using LEGO.AsyncAPI.Models;
using LEGO.AsyncApi.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiComponents> _componentsFixedFields = new FixedFieldMap<AsyncApiComponents>
        {
            {"schemas", (a, n) => a.Schemas = n.CreateMapWithReference(ReferenceType.Schema, LoadSchema)},
            {"responses", (a, n) => a.Responses = n.CreateMapWithReference(ReferenceType.Response, LoadResponse)},
            {"parameters", (a, n) => a.Parameters = n.CreateMapWithReference(ReferenceType.Parameter, LoadParameter)},
            {"examples", (a, n) => a.Examples = n.CreateMapWithReference(ReferenceType.Example, LoadExample)},
            {"requestBodies", (a, n) => a.RequestBodies = n.CreateMapWithReference(ReferenceType.RequestBody, LoadRequestBody)},
            {"headers", (a, n) => a.Headers = n.CreateMapWithReference(ReferenceType.Header, LoadHeader)},
            {"securitySchemes", (a, n) => a.SecuritySchemes = n.CreateMapWithReference(ReferenceType.SecurityScheme, LoadSecurityScheme)},
            {"links", (a, n) => a.Links = n.CreateMapWithReference(ReferenceType.Link, LoadLink)},
            {"callbacks", (a, n) => a.Callbacks = n.CreateMapWithReference(ReferenceType.Callback, LoadCallback)},
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