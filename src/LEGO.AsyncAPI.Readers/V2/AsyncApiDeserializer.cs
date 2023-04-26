// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Expressions;
    using LEGO.AsyncAPI.Extensions;
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

        private static void ProcessAnyFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyFieldMap<T> anyFieldMap)
        {
            foreach (var anyFieldName in anyFieldMap.Keys.ToList())
            {
                try
                {
                    mapNode.Context.StartObject(anyFieldName);

                    var convertedAsyncApiAny = AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                        anyFieldMap[anyFieldName].PropertyGetter(domainObject),
                        anyFieldMap[anyFieldName].SchemaGetter(domainObject));

                    anyFieldMap[anyFieldName].PropertySetter(domainObject, convertedAsyncApiAny);
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

        private static void ProcessAnyListFields<T>(
            MapNode mapNode,
            T domainObject,
            AnyListFieldMap<T> anyListFieldMap)
        {
            foreach (var anyListFieldName in anyListFieldMap.Keys.ToList())
            {
                try
                {
                    var newProperty = new List<IAsyncApiAny>();

                    mapNode.Context.StartObject(anyListFieldName);

                    foreach (var propertyElement in anyListFieldMap[anyListFieldName].PropertyGetter(domainObject))
                    {
                        newProperty.Add(
                            AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                                propertyElement,
                                anyListFieldMap[anyListFieldName].SchemaGetter(domainObject)));
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
                    var newProperty = new List<IAsyncApiAny>();

                    mapNode.Context.StartObject(anyMapFieldName);

                    foreach (var propertyMapElement in anyMapFieldMap[anyMapFieldName].PropertyMapGetter(domainObject))
                    {
                        mapNode.Context.StartObject(propertyMapElement.Key);

                        if (propertyMapElement.Value != null)
                        {
                            var any = anyMapFieldMap[anyMapFieldName].PropertyGetter(propertyMapElement.Value);

                            var newAny = AsyncApiAnyConverter.GetSpecificAsyncApiAny(
                                    any,
                                    anyMapFieldMap[anyMapFieldName].SchemaGetter(domainObject));

                            anyMapFieldMap[anyMapFieldName].PropertySetter(propertyMapElement.Value, newAny);
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

        private static RuntimeExpression LoadRuntimeExpression(ParseNode node)
        {
            var value = node.GetScalarValue();
            return RuntimeExpression.Build(value);
        }

        private static RuntimeExpressionAnyWrapper LoadRuntimeExpressionAnyWrapper(ParseNode node)
        {
            var value = node.GetScalarValue();

            if (value != null && value.StartsWith("$"))
            {
                return new RuntimeExpressionAnyWrapper
                {
                    Expression = RuntimeExpression.Build(value),
                };
            }

            return new RuntimeExpressionAnyWrapper
            {
                Any = AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny()),
            };
        }

        public static IAsyncApiAny LoadAny(ParseNode node)
        {
            return AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny());
        }

        internal static IAsyncApiExtension LoadExtension(string name, ParseNode node)
        {
            try
            {
                if (node.Context.ExtensionParsers.TryGetValue(name, out var parser))
                {
                    return parser(
                        AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny()));
                }
            }
            catch (AsyncApiException ex)
            {
                ex.Pointer = node.Context.GetLocation();
                node.Context.Diagnostic.Errors.Add(new AsyncApiError(ex));
            }

            return AsyncApiAnyConverter.GetSpecificAsyncApiAny(node.CreateAny());
        }

        private static string LoadString(ParseNode node)
        {
            return node.GetScalarValue();
        }
    }
}
