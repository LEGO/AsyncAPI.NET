// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Globalization;
    using System.IO;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public static class AsyncApiReferencableExtensions
    {
        ///// <summary>
        ///// Resolves a JSON Pointer with respect to an element, returning the referenced element.
        ///// </summary>
        ///// <param name="element">The referencable AsyncApi element on which to apply the JSON pointer</param>
        ///// <param name="pointer">a JSON Pointer [RFC 6901](https://tools.ietf.org/html/rfc6901).</param>
        ///// <returns>The element pointed to by the JSON pointer.</returns>
        //public static IReferenceable ResolveReference(this IReferenceable element, JsonPointer pointer)
        //{
        //    if (!pointer.Tokens.Any())
        //    {
        //        return element;
        //    }

        //    var propertyName = pointer.Tokens.FirstOrDefault();
        //    var mapKey = pointer.Tokens.ElementAtOrDefault(1);
        //    try
        //    {
        //        if (element.GetType() == typeof(AsyncApiHeader))
        //        {
        //            return ResolveReferenceOnHeaderElement((AsyncApiHeader)element, propertyName, mapKey, pointer);
        //        }
        //        if (element.GetType() == typeof(AsyncApiParameter))
        //        {
        //            return ResolveReferenceOnParameterElement((AsyncApiParameter)element, propertyName, mapKey, pointer);
        //        }
        //        if (element.GetType() == typeof(AsyncApiResponse))
        //        {
        //            return ResolveReferenceOnResponseElement((AsyncApiResponse)element, propertyName, mapKey, pointer);
        //        }
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
        //    }
        //    throw new AsyncApiException(string.Format(SRResource.InvalidReferenceId, pointer));
        //}
    }
}
