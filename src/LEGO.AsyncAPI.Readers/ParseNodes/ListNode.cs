// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using SharpYaml.Serialization;

    internal class ListNode : ParseNode, IEnumerable<ParseNode>
    {
        private readonly YamlSequenceNode nodeList;

        public ListNode(ParsingContext context, YamlSequenceNode sequenceNode) : base(
            context)
        {
            this.nodeList = sequenceNode;
        }

        public override List<T> CreateList<T>(Func<MapNode, T> map)
        {
            if (this.nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {this.nodeList.Start.Line} while parsing {typeof(T).Name}", this.nodeList);
            }

            return this.nodeList.Select(n => map(new MapNode(this.Context, n as YamlMappingNode)))
                .Where(i => i != null)
                .ToList();
        }

        public override List<IAsyncApiAny> CreateListOfAny()
        {
            return this.nodeList.Select(n => ParseNode.Create(this.Context, n).CreateAny())
                .Where(i => i != null)
                .ToList();
        }

        public override List<T> CreateSimpleList<T>(Func<ValueNode, T> map)
        {
            if (this.nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {this.nodeList.Start.Line} while parsing {typeof(T).Name}", this.nodeList);
            }

            return this.nodeList.Select(n => map(new ValueNode(this.Context, n))).ToList();
        }

        public IEnumerator<ParseNode> GetEnumerator()
        {
            return this.nodeList.Select(n => Create(this.Context, n)).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override IAsyncApiAny CreateAny()
        {
            var array = new AsyncApiArray();
            foreach (var node in this)
            {
                array.Add(node.CreateAny());
            }

            return array;
        }
    }
}
