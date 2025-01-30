// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System;

    public static class MessagePayloadExtensions
    {
        public static bool TryGetAs<T>(this IAsyncApiMessagePayload payload, out T result)
            where T : class, IAsyncApiMessagePayload
        {
            result = payload as T;
            return result != null;
        }

        public static T As<T>(this IAsyncApiMessagePayload payload)
            where T : class, IAsyncApiMessagePayload
        {
            return payload as T;
        }

        public static bool Is<T>(this IAsyncApiMessagePayload payload)
            where T : class, IAsyncApiMessagePayload
        {
            return payload is T;
        }
    }
}
