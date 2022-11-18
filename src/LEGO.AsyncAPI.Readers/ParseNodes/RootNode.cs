namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using SharpYaml.Serialization;

    internal class RootNode : ParseNode
    {
        private readonly YamlDocument yamlDocument;

        public RootNode(
            ParsingContext context,
            YamlDocument yamlDocument)
            : base(context)
        {
            this.yamlDocument = yamlDocument;
        }

        public ParseNode Find(JsonPointer referencePointer)
        {
            var yamlNode = referencePointer.Find(this.yamlDocument.RootNode);
            if (yamlNode == null)
            {
                return null;
            }

            return Create(this.Context, yamlNode);
        }

        public MapNode GetMap()
        {
            return new MapNode(this.Context, (YamlMappingNode)this.yamlDocument.RootNode);
        }
    }
}