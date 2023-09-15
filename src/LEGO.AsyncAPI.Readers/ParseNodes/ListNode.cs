// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Exceptions;

    public class ListNode : ParseNode, IEnumerable<ParseNode>
    {
        private readonly JsonArray nodeList;

        public ListNode(ParsingContext context, JsonArray sequenceNode)
            : base(
            context)
        {
            this.nodeList = sequenceNode;
        }

        public override List<T> CreateList<T>(Func<MapNode, T> map)
        {
            if (this.nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list while parsing {typeof(T).Name}");
            }

            return this.nodeList.Select(n => map(new MapNode(this.Context, n as JsonObject)))
                .Where(i => i != null)
                .ToList();
        }

        public override List<AsyncApiAny> CreateListOfAny()
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
                    $"Expected list while parsing {typeof(T).Name}");
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

        public override AsyncApiAny CreateAny()
        {
            return new AsyncApiAny(this.nodeList);
        }
    }
}
