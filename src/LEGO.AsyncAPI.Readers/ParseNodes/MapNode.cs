using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.Exceptions;
using SharpYaml.Schemas;
using SharpYaml.Serialization;

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    internal class MapNode : ParseNode, IEnumerable<PropertyNode>
    {
        private readonly YamlMappingNode _node;
        private readonly List<PropertyNode> _nodes;

        public MapNode(ParsingContext context, string yamlString) :
            this(context, (YamlMappingNode)YamlHelper.ParseYamlString(yamlString))
        {
        }

        public MapNode(ParsingContext context, YamlNode node) : base(
            context)
        {
            if (!(node is YamlMappingNode mapNode))
            {
                throw new AsyncApiReaderException("Expected map.", Context);
            }

            this._node = mapNode;

            _nodes = this._node.Children
                .Select(kvp => new PropertyNode(Context, kvp.Key.GetScalarValue(), kvp.Value))
                .Cast<PropertyNode>()
                .ToList();
        }

        public PropertyNode this[string key]
        {
            get
            {
                YamlNode node;
                if (this._node.Children.TryGetValue(new YamlScalarNode(key), out node))
                {
                    return new PropertyNode(Context, key, node);
                }

                return null;
            }
        }

        public override Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            var yamlMap = _node;
            if (yamlMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", Context);
            }

            var nodes = yamlMap.Select(
                n => {
                    
                    var key = n.Key.GetScalarValue();
                    T value;
                    try
                    {
                        Context.StartObject(key);
                        value = n.Value as YamlMappingNode == null
                          ? default(T)
                          : map(new MapNode(Context, n.Value as YamlMappingNode));
                    } 
                    finally
                    {
                        Context.EndObject();
                    }
                    return new
                    {
                        key = key,
                        value = value
                    };
                });

            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public override Dictionary<string, T> CreateMapWithReference<T>(
            ReferenceType referenceType,
            Func<MapNode, T> map) 
        {
            var yamlMap = _node;
            if (yamlMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", Context);
            }

            var nodes = yamlMap.Select(
                n =>
                {
                    var key = n.Key.GetScalarValue();
                    (string key, T value) entry;
                    try
                    {
                        Context.StartObject(key);
                        entry = (
                            key: key,
                            value: map(new MapNode(Context, (YamlMappingNode)n.Value))
                        );
                        if (entry.value == null)
                        {
                            return default; 
                        }

                        if (entry.value.Reference == null)
                        {
                            entry.value.Reference = new AsyncApiReference()
                            {
                                Type = referenceType,
                                Id = entry.key
                            };
                        }
                     }
                    finally
                    {
                        Context.EndObject();
                    }
                    return entry;
                }
                );
            return nodes.Where(n => n != default).ToDictionary(k => k.key, v => v.value);
        }

        public override Dictionary<string, T> CreateSimpleMap<T>(Func<ValueNode, T> map)
        {
            var yamlMap = _node;
            if (yamlMap == null)
            { 
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", Context);
            }

            var nodes = yamlMap.Select(
                n =>
                {
                    var key = n.Key.GetScalarValue();
                    try
                    {
                        Context.StartObject(key);
                        YamlScalarNode scalarNode = n.Value as YamlScalarNode;
                        if (scalarNode == null)
                        {
                            throw new AsyncApiReaderException($"Expected scalar while parsing {typeof(T).Name}", Context);
                        }
                        return (key, value: map(new ValueNode(Context, (YamlScalarNode)n.Value)));
                    } finally {
                        Context.EndObject();
                    }
                });
            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public IEnumerator<PropertyNode> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        public override string GetRaw()
        {
            var x = new Serializer(new SerializerSettings(new JsonSchema()) { EmitJsonComptible = true });
            return x.Serialize(_node);
        }

        public T GetReferencedObject<T>(ReferenceType referenceType, string referenceId)
            where T : IAsyncApiReferenceable, new()
        {
            return new T()
            {
                UnresolvedReference = true,
                Reference = Context.VersionService.ConvertToAsyncApiReference(referenceId, referenceType)
            };
        }

        public string GetReferencePointer()
        {
            YamlNode refNode;

            if (!_node.Children.TryGetValue(new YamlScalarNode("$ref"), out refNode))
            {
                return null;
            }

            return refNode.GetScalarValue();
        }

        public string GetScalarValue(ValueNode key)
        {
            var scalarNode = _node.Children[new YamlScalarNode(key.GetScalarValue())] as YamlScalarNode;
            if (scalarNode == null)
            {
                throw new AsyncApiReaderException($"Expected scalar at line {_node.Start.Line} for key {key.GetScalarValue()}", Context);
            }

            return scalarNode.Value;
        }

        public override IAsyncApiAny CreateAny()
        {
            var apiObject = new AsyncApiObject();
            foreach (var node in this)
            {
                apiObject.Add(node.Name, node.Value.CreateAny());
            }

            return apiObject;
        }
    }
}
