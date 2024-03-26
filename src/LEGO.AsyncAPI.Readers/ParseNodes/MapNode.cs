// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;

    public class MapNode : ParseNode, IEnumerable<PropertyNode>
    {
        private readonly JsonObject node;
        private readonly List<PropertyNode> nodes;

        public MapNode(ParsingContext context, string jsonString)
            : this(context, JsonHelper.ParseJsonString(jsonString))
        {
        }

        public MapNode(ParsingContext context, JsonNode node)
            : base(
            context)
        {
            if (!(node is JsonObject mapNode))
            {
                throw new AsyncApiReaderException("Expected map.", this.Context);
            }

            this.node = mapNode;

            this.nodes = this.node
                .Select(node => new PropertyNode(this.Context, node.Key, node.Value))
                .ToList();
        }

        public PropertyNode this[string key]
        {
            get
            {
                if (this.node.TryGetPropertyValue(key, out var node))
                {
                    return new PropertyNode(this.Context, key, node);
                }

                return null;
            }
        }

        public override Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            var jsonMap = this.node;
            if (jsonMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = jsonMap.Select(
                n =>
                {
                    var key = n.Key;
                    T value;
                    try
                    {
                        this.Context.StartObject(key);
                        value = n.Value is JsonObject
                          ? map(new MapNode(this.Context, n.Value))
                          : default(T);
                    }
                    finally
                    {
                        this.Context.EndObject();
                    }

                    return new
                    {
                        key,
                        value,
                    };
                });

            return nodes.ToDictionary(k => k.key, v => v.value);
        }

        public override Dictionary<string, T> CreateMapWithReference<T>(
            ReferenceType referenceType,
            Func<MapNode, T> map)
        {
            var jsonMap = this.node;
            if (jsonMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = jsonMap.Select(
                n =>
                {
                    var key = n.Key;
                    (string key, T value) entry;
                    try
                    {
                        this.Context.StartObject(key);
                        entry = (
                            key,
                            value: map(new MapNode(this.Context, n.Value))
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
            var jsonMap = this.node;
            if (jsonMap == null)
            {
                throw new AsyncApiReaderException($"Expected map while parsing {typeof(T).Name}", this.Context);
            }

            var nodes = jsonMap.Select(
                n =>
                {
                    var key = n.Key;
                    try
                    {
                        this.Context.StartObject(key);
                        JsonValue scalarNode = n.Value as JsonValue;
                        if (scalarNode == null)
                        {
                            throw new AsyncApiReaderException($"Expected scalar while parsing {typeof(T).Name}", this.Context);
                        }

                        return (key, value: map(new ValueNode(this.Context, n.Value)));
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
            return JsonSerializer.Serialize(this.node);
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
            if (!this.node.TryGetPropertyValue("$ref", out JsonNode refNode))
            {
                return null;
            }

            var scalarNode = refNode is JsonValue value ? value : throw new AsyncApiException($"Expected scalar value");
            return Convert.ToString(scalarNode.GetValue<object>(), this.Context.CultureInfo);
        }

        public string GetScalarValue(ValueNode key)
        {
            var node = this.node[key.GetScalarValue()] is JsonValue jsonValue
                ? jsonValue
                : throw new AsyncApiReaderException($"Expected scalar value while parsing {key.GetScalarValue()}", this.Context);

            var scalarNode = node is JsonValue value ? value : throw new AsyncApiException($"Expected scalar value");
            return Convert.ToString(scalarNode.GetValue<object>(), this.Context.CultureInfo);
        }

        public override AsyncApiAny CreateAny()
        {
            return new AsyncApiAny(this.node);
        }
    }
}
