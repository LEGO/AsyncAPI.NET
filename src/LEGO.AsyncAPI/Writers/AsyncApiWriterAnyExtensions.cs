// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class AsyncApiWriterAnyExtensions
    {
        /// <summary>
        /// Write the specification extensions
        /// </summary>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="extensions">The specification extensions.</param>
        /// <param name="specVersion">Version of the AsyncApi specification that that will be output.</param>
        public static void WriteExtensions(this IAsyncApiWriter writer, IDictionary<string, IAsyncApiExtension> extensions, AsyncApiVersion specVersion)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (extensions != null)
            {
                foreach (var item in extensions)
                {
                    writer.WritePropertyName(item.Key);
                    item.Value.Write(writer, specVersion);
                }
            }
        }

        /// <summary>
        /// Write the <see cref="IAsyncApiAny"/> value.
        /// </summary>
        /// <typeparam name="T">The AsyncApi Any type.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="any">The Any value</param>
        public static void WriteAny<T>(this IAsyncApiWriter writer, T any) where T : IAsyncApiAny
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (any == null)
            {
                writer.WriteNull();
                return;
            }

            switch (any.AnyType)
            {
                case AnyType.Array: // Array
                    writer.WriteArray(any as AsyncApiArray);
                    break;

                case AnyType.Object: // Object
                    writer.WriteObject(any as AsyncApiObject);
                    break;

                case AnyType.Primitive: // Primitive
                    writer.WritePrimitive(any as IAsyncApiPrimitive);
                    break;

                case AnyType.Null: // null
                    writer.WriteNull();
                    break;

                default:
                    break;
            }
        }

        private static void WriteArray(this IAsyncApiWriter writer, AsyncApiArray array)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            writer.WriteStartArray();

            foreach (var item in array)
            {
                writer.WriteAny(item);
            }

            writer.WriteEndArray();
        }

        private static void WriteObject(this IAsyncApiWriter writer, AsyncApiObject entity)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            writer.WriteStartObject();

            foreach (var item in entity)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteAny(item.Value);
            }

            writer.WriteEndObject();
        }

        private static void WritePrimitive(this IAsyncApiWriter writer, IAsyncApiPrimitive primitive)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (primitive is null)
            {
                throw new ArgumentNullException(nameof(primitive));
            }

            primitive.Write(writer, AsyncApiVersion.AsyncApi2_3_0);
        }
    }
}
