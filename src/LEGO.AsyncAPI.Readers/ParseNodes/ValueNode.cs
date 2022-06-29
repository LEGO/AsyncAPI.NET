// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using SharpYaml;
    using SharpYaml.Serialization;

    internal class ValueNode : ParseNode
    {
        private readonly YamlScalarNode node;

        public ValueNode(ParsingContext context, YamlNode node)
            : base(
            context)
        {
            if (!(node is YamlScalarNode scalarNode))
            {
                throw new AsyncApiReaderException("Expected a value.", node);
            }

            this.node = scalarNode;
        }

        public override string GetScalarValue()
        {
            return this.node.Value;
        }

        public override IAsyncApiAny CreateAny()
        {
            var value = this.GetScalarValue();
            return new AsyncApiString(value, this.node.Style == ScalarStyle.SingleQuoted || this.node.Style == ScalarStyle.DoubleQuoted || this.node.Style == ScalarStyle.Literal || this.node.Style == ScalarStyle.Folded);
        }
    }
}