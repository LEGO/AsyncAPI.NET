// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Bindings;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using SharpYaml.Serialization;

    internal abstract class ParseNode
    {
        protected ParseNode(ParsingContext parsingContext)
        {
            this.Context = parsingContext;
        }

        public ParsingContext Context { get; }

        public MapNode CheckMapNode(string nodeName)
        {
            if (!(this is MapNode mapNode))
            {
                throw new AsyncApiReaderException($"{nodeName} must be a map/object", this.Context);
            }

            return mapNode;
        }

        public static ParseNode Create(ParsingContext context, YamlNode node)
        {
            if (node is YamlSequenceNode listNode)
            {
                return new ListNode(context, listNode);
            }

            if (node is YamlMappingNode mapNode)
            {
                return new MapNode(context, mapNode);
            }

            return new ValueNode(context, node as YamlScalarNode);
        }

        public virtual List<T> CreateList<T>(Func<MapNode, T> map)
        {
            throw new AsyncApiReaderException("Cannot create list from this type of node.", this.Context);
        }

        public virtual Dictionary<string, T> CreateMap<T>(Func<MapNode, T> map)
        {
            throw new AsyncApiReaderException("Cannot create map from this type of node.", this.Context);
        }

        public virtual Dictionary<string, T> CreateBindingMapWithReference<T>(
            ReferenceType referenceType,
            Func<ParseNode, T> map)
            where T : class, IBinding
        {
            throw new AsyncApiReaderException("Cannot create map from this reference.", this.Context);
        }

        public virtual Dictionary<string, T> CreateMapWithReference<T>(
            ReferenceType referenceType,
            Func<MapNode, T> map)
            where T : class, IAsyncApiReferenceable
        {
            throw new AsyncApiReaderException("Cannot create map from this reference.", this.Context);
        }

        public virtual List<T> CreateSimpleList<T>(Func<ValueNode, T> map)
        {
            throw new AsyncApiReaderException("Cannot create simple list from this type of node.", this.Context);
        }

        public virtual Dictionary<string, T> CreateSimpleMap<T>(Func<ValueNode, T> map)
        {
            throw new AsyncApiReaderException("Cannot create simple map from this type of node.", this.Context);
        }

        public virtual IAsyncApiAny CreateAny()
        {
            throw new AsyncApiReaderException("Cannot create an Any object this type of node.", this.Context);
        }

        public virtual string GetRaw()
        {
            throw new AsyncApiReaderException("Cannot get raw value from this type of node.", this.Context);
        }

        public virtual string GetScalarValue()
        {
            throw new AsyncApiReaderException("Cannot create a scalar value from this type of node.", this.Context);
        }

        public virtual string GetScalarValueOrDefault(string defaultValue)
        {
            throw new AsyncApiReaderException("Cannot create a scalar value from this type of node.", this.Context);
        }

        public virtual bool GetBooleanValue()
        {
            throw new AsyncApiReaderException("Cannot create a scalar value from this type of node.", this.Context);
        }

        public virtual int GetIntegerValue()
        {
            throw new AsyncApiReaderException("Cannot create a scalar value from this type of node.", this.Context);
        }

        public virtual long GetLongValue()
        {
            throw new AsyncApiReaderException("Cannot create a scalar value from this type of node.", this.Context);
        }

        public virtual List<IAsyncApiAny> CreateListOfAny()
        {
            throw new AsyncApiReaderException("Cannot create a list from this type of node.", this.Context);
        }
    }
}
