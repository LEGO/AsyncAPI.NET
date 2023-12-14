// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class AsyncApiWriterAnyExtensions
    {
        /// <summary>
        /// Write the specification extensions.
        /// </summary>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="extensions">The specification extensions.</param>
        /// <param name="specVersion">Version of the AsyncApi specification that that will be output.</param>
        public static void WriteExtensions(this IAsyncApiWriter writer, IDictionary<string, IAsyncApiExtension> extensions)
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
                    if (item.Value == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        item.Value.Write(writer);
                    }
                }
            }
        }

        /// <summary>
        /// Write the <see cref="AsyncApiAny"/> value.
        /// </summary>
        /// <typeparam name="T">The AsyncApi Any type.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="any">The Any value.</param>
        public static void WriteAny(this IAsyncApiWriter writer, AsyncApiAny any)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (any.GetNode() == null)
            {
                writer.WriteNull();
                return;
            }

            var node = any.GetNode();

            var element = JsonDocument.Parse(node.ToJsonString()).RootElement;
            switch (element.ValueKind)
            {
                case JsonValueKind.Array: // Array
                    writer.WriteArray(node as JsonArray);
                    break;

                case JsonValueKind.Object: // Object
                    writer.WriteObject(node as JsonObject);
                    break;

                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.False or JsonValueKind.True:
                    writer.WritePrimitive(element);
                    break;

                case JsonValueKind.Null: // null
                    writer.WriteNull();
                    break;
                default:
                    break;
            }
        }

        private static void WriteArray(this IAsyncApiWriter writer, JsonArray array)
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
                writer.WriteAny(new AsyncApiAny(item));
            }

            writer.WriteEndArray();
        }

        private static void WriteObject(this IAsyncApiWriter writer, JsonObject entity)
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
                writer.WriteAny(new AsyncApiAny(item.Value));
            }

            writer.WriteEndObject();
        }

        private static void WritePrimitive(this IAsyncApiWriter writer, JsonElement primitive)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (primitive.ValueKind == JsonValueKind.String)
            {
                if (primitive.TryGetDateTime(out var dateTime))
                {
                    writer.WriteValue(dateTime);
                }
                else if (primitive.TryGetDateTimeOffset(out var dateTimeOffset))
                {
                    writer.WriteValue(dateTimeOffset);
                }
                else
                {
                    writer.WriteValue(primitive.GetString());
                }
            }

            if (primitive.ValueKind == JsonValueKind.Number)
            {
                if (primitive.TryGetDecimal(out var decimalValue))
                {
                    writer.WriteValue(decimalValue);
                }
                else if (primitive.TryGetDouble(out var doubleValue))
                {
                    writer.WriteValue(doubleValue);
                }
                else if (primitive.TryGetInt64(out var longValue))
                {
                    writer.WriteValue(longValue);
                }
                else if (primitive.TryGetInt32(out var intValue))
                {
                    writer.WriteValue(intValue);
                }
            }
            if (primitive.ValueKind is JsonValueKind.True or JsonValueKind.False)
            {
                writer.WriteValue(primitive.GetBoolean());
            }
        }
    }
}
