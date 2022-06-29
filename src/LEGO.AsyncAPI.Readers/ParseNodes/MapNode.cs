// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
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

    internal class MapNode : ParseNode, IEnumerable<PropertyNode>
    {
        private readonly YamlMappingNode node;
        private readonly List<PropertyNode> nodes;

        public MapNode(ParsingContext context, string yamlString) :
            this(context, (YamlMappingNode)YamlHelper.ParseYamlString(yamlString))
        {
        }

        public MapNode(ParsingContext context, YamlNode node) : base(
            context)
        {
            if (!(node is YamlMappingNode mapNode))
            {
                throw new AsyncApiReaderException("Expected map.", this.Context);
            }

            this.node = mapNode;

            this.nodes = this.node.Children
                .Select(kvp => new PropertyNode(this.Context, kvp.Key.GetScalarValue(), kvp.Value))
                .Cast<PropertyNode>()
                .ToList();
        }

        public PropertyNode this[string key]
        {
            get
            {
                YamlNode node;
                if (this.node.Children.TryGetValue(new YamlScalarNode(key), out node))
                {
                    return new PropertyNode(this.Context, key, node);
                }

                return null;
            }
        }

        public override Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            var yamlMap = this.node;
            if (yamlMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = yamlMap.Select(
                n =>
                {
                    var key = n.Key.GetScalarValue();
                    T value;
                    try
                    {
                        this.Context.StartObject(key);
                        value = n.Value as YamlMappingNode == null
                          ? default(T)
                          : map(new MapNode(this.Context, n.Value as YamlMappingNode));
                    }
                    finally
                    {
                        this.Context.EndObject();
                    }

                    return new
                    {
                        key = key,
                        value = value,
                    };
                });

            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public override Dictionary<string, T> CreateMapWithReference<T>(
            ReferenceType referenceType,
            Func<MapNode, T> map)
        {
            var yamlMap = this.node;
            if (yamlMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = yamlMap.Select(
                n =>
                {
                    var key = n.Key.GetScalarValue();
                    (string key, T value) entry;
                    try
                    {
                        this.Context.StartObject(key);
                        entry = (
                            key: key,
                            value: map(new MapNode(this.Context, (YamlMappingNode)n.Value))
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
                                Id = entry.key,
                            };
                        }
                    }
                    finally
                    {
                        this.Context.EndObject();
                    }

                    return entry;
                }
                );
            return nodes.Where(n => n != default).ToDictionary(k => k.key, v => v.value);
        }

        public override Dictionary<string, T> CreateSimpleMap<T>(Func<ValueNode, T> map)
        {
            var yamlMap = this.node;
            if (yamlMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = yamlMap.Select(
                n =>
                {
                    var key = n.Key.GetScalarValue();
                    try
                    {
                        this.Context.StartObject(key);
                        YamlScalarNode scalarNode = n.Value as YamlScalarNode;
                        if (scalarNode == null)
                        {
                            throw new AsyncApiReaderException($"Expected scalar while parsing {typeof(T).Name}", this.Context);
                        }

                        return (key, value: map(new ValueNode(this.Context, (YamlScalarNode)n.Value)));
                    }
                    finally
                    {
                        this.Context.EndObject();
                    }
                });
            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public IEnumerator<PropertyNode> GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.nodes.GetEnumerator();
        }

        public override string GetRaw()
        {
            var x = new Serializer(new SerializerSettings(new JsonSchema()) { EmitJsonComptible = true });
            return x.Serialize(this.node);
        }

        public T GetReferencedObject<T>(ReferenceType referenceType, string referenceId)
            where T : IAsyncApiReferenceable, new()
        {
            return new T()
            {
                UnresolvedReference = true,
                Reference = this.Context.VersionService.ConvertToAsyncApiReference(referenceId, referenceType),
            };
        }

        public string GetReferencePointer()
        {
            YamlNode refNode;

            if (!this.node.Children.TryGetValue(new YamlScalarNode("$ref"), out refNode))
            {
                return null;
            }

            return refNode.GetScalarValue();
        }

        public string GetScalarValue(ValueNode key)
        {
            var scalarNode = this.node.Children[new YamlScalarNode(key.GetScalarValue())] as YamlScalarNode;
            if (scalarNode == null)
            {
                throw new AsyncApiReaderException($"Expected scalar at line {this.node.Start.Line} for key {key.GetScalarValue()}", this.Context);
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
