namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncAPIDocument> _asyncAPIFixedFields = new FixedFieldMap<AsyncAPIDocument>
        {
            {
                "openapi", (o, n) =>
                {

            },
            {"info", (o, n) => o.Info = LoadInfo(n)},
            {"servers", (o, n) => o.Servers = n.CreateList(LoadServer)},
            {"paths", (o, n) => o.Paths = LoadPaths(n)},
            {"components", (o, n) => o.Components = LoadComponents(n)},
            {"tags", (o, n) => {o.Tags = n.CreateList(LoadTag);
                foreach (var tag in o.Tags)
    {
                    tag.Reference = new AsyncAPIReference()
                    {
                        Id = tag.Name,
                        Type = ReferenceType.Tag
                    };
    }
            } },
            {"externalDocs", (o, n) => o.ExternalDocs = LoadExternalDocs(n)},
            {"security", (o, n) => o.SecurityRequirements = n.CreateList(LoadSecurityRequirement)}
        };

        private static PatternFieldMap<AsyncAPIDocument> _asyncAPIPatternFields = new PatternFieldMap<AsyncAPIDocument>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n))}
        };

        public static AsyncAPIDocument LoadAsyncAPI(RootNode rootNode)
        {
            var asyncAPIdoc = new AsyncAPIDocument();

            var asyncAPINode = rootNode.GetMap();

            ParseMap(asyncAPINode, asyncAPIdoc, _asyncAPIFixedFields, _asyncAPIPatternFields);

            return asyncAPIdoc;
        }
    }
}
