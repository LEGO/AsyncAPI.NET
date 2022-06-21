using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.Exceptions;
using SharpYaml.Serialization;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class ListNode : ParseNode, IEnumerable<ParseNode>
    {
        private readonly YamlSequenceNode _nodeList;

        public ListNode(ParsingContext context, YamlSequenceNode sequenceNode) : base(
            context)
        {
            _nodeList = sequenceNode;
        }

        public override List<T> CreateList<T>(Func<MapNode, T> map)
        {
            if (_nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {_nodeList.Start.Line} while parsing {typeof(T).Name}", _nodeList);
            }

            return _nodeList.Select(n => map(new MapNode(Context, n as YamlMappingNode)))
                .Where(i => i != null)
                .ToList();
        }

        public override List<IAsyncApiAny> CreateListOfAny()
        {
            return _nodeList.Select(n => ParseNode.Create(Context, n).CreateAny())
                .Where(i => i != null)
                .ToList();
        }

        public override List<T> CreateSimpleList<T>(Func<ValueNode, T> map)
        {
            if (_nodeList == null)
            {
                throw new AsyncApiReaderException(
                    $"Expected list at line {_nodeList.Start.Line} while parsing {typeof(T).Name}", _nodeList);
            }

            return _nodeList.Select(n => map(new ValueNode(Context, n))).ToList();
        }

        public IEnumerator<ParseNode> GetEnumerator()
        {
            return _nodeList.Select(n => Create(Context, n)).ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
