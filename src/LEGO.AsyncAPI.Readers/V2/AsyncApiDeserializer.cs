// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        internal static void ParseMap<T>(
            MapNode mapNode,
            T domainObject,
            FixedFieldMap<T> fixedFieldMap,
            PatternFieldMap<T> patternFieldMap)
        {
            if (mapNode == null)
            {
                return;
            }

            foreach (var propertyNode in mapNode)
            {
                propertyNode.ParseField(domainObject, fixedFieldMap, patternFieldMap);
            }
        }

        internal static void ProcessAnyFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyFieldMap<T> anyFieldMap)
        {
            foreach (var anyFieldName in anyFieldMap.Keys.ToList())
            {
                try
                {
                    mapNode.Context.StartObject(anyFieldName);

                    var anyFieldValue = anyFieldMap[anyFieldName].PropertyGetter(domainObject);
                    if (anyFieldValue == null)
                    {
                        anyFieldMap[anyFieldName].PropertySetter(domainObject, null);
                    }
                    else
                    {
                        anyFieldMap[anyFieldName].PropertySetter(domainObject, anyFieldValue);
                    }
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        internal static void ProcessAnyListFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyListFieldMap<T> anyListFieldMap)
        {
            foreach (var anyListFieldName in anyListFieldMap.Keys.ToList())
            {
                try
                {
                    var newProperty = new List<AsyncApiAny>();

                    mapNode.Context.StartObject(anyListFieldName);

                    foreach (var propertyElement in anyListFieldMap[anyListFieldName].PropertyGetter(domainObject))
                    {
                        newProperty.Add(propertyElement);
                    }

                    anyListFieldMap[anyListFieldName].PropertySetter(domainObject, newProperty);
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        private static void ProcessAnyMapFields<T, U>(
            MapNode mapNode,
            T domainObject,
            AnyMapFieldMap<T, U> anyMapFieldMap)
        {
            foreach (var anyMapFieldName in anyMapFieldMap.Keys.ToList())
            {
                try
                {
                    var newProperty = new List<AsyncApiAny>();

                    mapNode.Context.StartObject(anyMapFieldName);

                    foreach (var propertyMapElement in anyMapFieldMap[anyMapFieldName].PropertyMapGetter(domainObject))
                    {
                        mapNode.Context.StartObject(propertyMapElement.Key);

                        if (propertyMapElement.Value != null)
                        {
                            var any = anyMapFieldMap[anyMapFieldName].PropertyGetter(propertyMapElement.Value);

                            anyMapFieldMap[anyMapFieldName].PropertySetter(propertyMapElement.Value, any);
                        }
                    }
                }
                catch (AsyncApiException exception)
                {
                    exception.Pointer = mapNode.Context.GetLocation();
                    mapNode.Context.Diagnostic.Errors.Add(new AsyncApiError(exception));
                }
                finally
                {
                    mapNode.Context.EndObject();
                }
            }
        }

        public static AsyncApiAny LoadAny(ParseNode node)
        {
            return node.CreateAny();
        }

        public static IAsyncApiExtension LoadExtension(string name, ParseNode node)
        {
            try
            {
                if (node.Context.ExtensionParsers.TryGetValue(name, out var parser))
                {
                    return parser(node.CreateAny());
                }
            }
            catch (AsyncApiException ex)
            {
                ex.Pointer = node.Context.GetLocation();
                node.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            return node.CreateAny();
        }

        private static string LoadString(ParseNode node)
        {
            return node.GetScalarValue();
        }
    }
}
