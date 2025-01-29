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

        public static bool TryGetAs<T>(this AsyncApiAvroSchema avroSchema, out T result)
    where T : AsyncApiAvroSchema
        {
            result = avroSchema as T;
            return result != null;
        }

        public static bool Is<T>(this IAsyncApiMessagePayload payload)
            where T : class, IAsyncApiMessagePayload
        {
            return payload is T;
        }

        public static bool Is<T>(this AsyncApiAvroSchema schema)
            where T : AsyncApiAvroSchema
        {
            return schema is T;
        }
    }
}
