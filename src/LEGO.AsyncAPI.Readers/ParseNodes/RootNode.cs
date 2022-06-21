using SharpYaml.Serialization;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class RootNode : ParseNode
    {
        private readonly YamlDocument _yamlDocument;

        public RootNode(
            ParsingContext context,
            YamlDocument yamlDocument) : base(context)
        {
            _yamlDocument = yamlDocument;
        }

        public ParseNode Find(JsonPointer referencePointer)
        {
            var yamlNode = referencePointer.Find(_yamlDocument.RootNode);
            if (yamlNode == null)
            {
                return null;
            }

            return Create(Context, yamlNode);
        }

        public MapNode GetMap()
        {
            return new MapNode(Context, (YamlMappingNode)_yamlDocument.RootNode);
        }
    }
}