using SharpYaml;
using SharpYaml.Serialization;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    internal class ValueNode : ParseNode
    {
        private readonly YamlScalarNode _node;

        public ValueNode(ParsingContext context, YamlNode node) : base(
            context)
        {
            if (!(node is YamlScalarNode scalarNode))
            {
                throw new AsyncApiReaderException("Expected a value.", node);
            }
            _node = scalarNode;
        }

        public override string GetScalarValue()
        {
            return _node.Value;
        }
        
        public override IAsyncApiAny CreateAny()
        {
            var value = GetScalarValue();
            return new AsyncApiString(value, this._node.Style == ScalarStyle.SingleQuoted || this._node.Style == ScalarStyle.DoubleQuoted || this._node.Style == ScalarStyle.Literal || this._node.Style == ScalarStyle.Folded);
        }
    }
}