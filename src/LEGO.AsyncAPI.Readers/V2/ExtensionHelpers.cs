// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public static class ExtensionHelpers
    {
        public static PatternFieldMap<T> GetExtensionsFieldMap<T>() where T : IAsyncApiExtensible
        {
            return new ()
            {
                {
                    s => s.StartsWith("x-"),
                    (a, p, n) => a.AddExtension(p, LoadExtension(p, n))
                },
            };
        }

        public static IAsyncApiExtension LoadExtension(string name, ParseNode node)
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
    }
}