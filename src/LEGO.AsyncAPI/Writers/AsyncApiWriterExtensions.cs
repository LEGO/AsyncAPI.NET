// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Json.Schema;
    using LEGO.AsyncAPI.Models.Interfaces;

    public static class AsyncApiWriterExtensions
    {
        /// <summary>
        /// Write a string property.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void WriteOptionalProperty(this IAsyncApiWriter writer, string name, string value)
        {
            if (value == null)
            {
                return;
            }

            CheckArguments(writer, name);
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        /// <summary>
        /// Write required string property.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void WriteRequiredProperty(this IAsyncApiWriter writer, string name, string value)
        {
            CheckArguments(writer, name);
            writer.WritePropertyName(name);
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(value);
            }
        }

        /// <summary>
        /// Write a boolean property.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="defaultValue">The default boolean value.</param>
        public static void WriteOptionalProperty(this IAsyncApiWriter writer, string name, bool value, bool defaultValue = false)
        {
            if (value == defaultValue)
            {
                return;
            }

            CheckArguments(writer, name);
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        /// <summary>
        /// Write a boolean property.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="defaultValue">The default boolean value.</param>
        public static void WriteOptionalProperty(
            this IAsyncApiWriter writer,
            string name,
            bool? value,
            bool defaultValue = false)
        {
            if (value == null || value.Value == defaultValue)
            {
                return;
            }

            CheckArguments(writer, name);
            writer.WritePropertyName(name);
            writer.WriteValue(value.Value);
        }

        /// <summary>
        /// Write a primitive property.
        /// </summary>
        /// <typeparam name="T"><see cref="T"/>.</typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void WriteOptionalProperty<T>(this IAsyncApiWriter writer, string name, T? value)
            where T : struct
        {
            if (value == null)
            {
                return;
            }

            writer.WriteRequiredProperty(name, value.Value);
        }

        /// <summary>
        /// Write a string/number property.
        /// </summary>
        /// <typeparam name="T"><see cref="T"/>.</typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        public static void WriteRequiredProperty<T>(this IAsyncApiWriter writer, string name, T value)
            where T : struct
        {
            CheckArguments(writer, name);
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        /// <summary>
        /// Write the optional AsyncApi object/element.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="action">The proprety value writer action.</param>
        public static void WriteOptionalObject<T>(
            this IAsyncApiWriter writer,
            string name,
            T value,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            if (value != null)
            {
                if (value is IAsyncApiReferenceable refer && refer.Reference != null)
                {
                    writer.WriteRequiredObject(name, value, action);
                    return;
                }

                var values = value as IEnumerable;
                if (values != null && !values.GetEnumerator().MoveNext())
                {
                    return; // Don't render optional empty collections
                }

                writer.WriteRequiredObject(name, value, action);
            }
        }

        public static void WriteOptionalObject(
            this IAsyncApiWriter writer,
            string name,
            JsonSchema value,
            Action<IAsyncApiWriter, JsonSchema> action)
        {
            if (value != null)
            {
                if (value is IAsyncApiReferenceable refer && refer.Reference != null)
                {
                    writer.WriteRequiredObject(name, value, action);
                    return;
                }

                var values = value as IEnumerable;
                if (values != null && !values.GetEnumerator().MoveNext())
                {
                    return; // Don't render optional empty collections
                }

                writer.WriteRequiredObject(name, value, action);
            }
        }

        /// <summary>
        /// Write the required AsyncApi object/element.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <param name="action">The proprety value writer action.</param>
        public static void WriteRequiredObject<T>(
            this IAsyncApiWriter writer,
            string name,
            T value,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            CheckArguments(writer, name, action);

            writer.WritePropertyName(name);
            if (value != null)
            {
                action(writer, value);
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
        }

        public static void WriteRequiredObject(
            this IAsyncApiWriter writer,
            string name,
            JsonSchema value,
            Action<IAsyncApiWriter, JsonSchema> action)
        {
            CheckArguments(writer, name, action);

            writer.WritePropertyName(name);
            if (value != null)
            {
                action(writer, value);
            }
            else
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
        }

        /// <summary>
        /// Write the optional of collection string.
        /// </summary>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The collection values.</param>
        /// <param name="action">The collection string writer action.</param>
        public static void WriteOptionalCollection(
            this IAsyncApiWriter writer,
            string name,
            IEnumerable<string> elements,
            Action<IAsyncApiWriter, string> action)
        {
            if (elements != null && elements.Any())
            {
                writer.WriteCollectionInternal(name, elements, action);
            }
        }

        public static void WriteOptionalCollection(
            this IAsyncApiWriter writer,
            string name,
            IEnumerable<JsonSchema> elements,
            Action<IAsyncApiWriter, JsonSchema> action)
        {
            if (elements != null && elements.Any())
            {
                writer.WriteCollectionInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the optional AsyncApi object/element collection.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The collection values.</param>
        /// <param name="action">The collection element writer action.</param>
        public static void WriteOptionalCollection<T>(
            this IAsyncApiWriter writer,
            string name,
            IEnumerable<T> elements,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            if (elements != null && elements.Any())
            {
                writer.WriteCollectionInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the required AsyncApi object/element collection.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The collection values.</param>
        /// <param name="action">The collection element writer action.</param>
        public static void WriteRequiredCollection<T>(
            this IAsyncApiWriter writer,
            string name,
            IEnumerable<T> elements,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            writer.WriteCollectionInternal(name, elements, action);
        }

        /// <summary>
        /// Write the optional AsyncApi element map (string to string mapping).
        /// </summary>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The map values.</param>
        /// <param name="action">The map element writer action.</param>
        public static void WriteOptionalMap(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, string> elements,
            Action<IAsyncApiWriter, string> action)
        {
            if (elements != null && elements.Any())
            {
                writer.WriteMapInternal(name, elements, action);
            }
        }

        public static void WriteOptionalMap(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, JsonSchema> elements,
            Action<IAsyncApiWriter, string, JsonSchema> action)
        {
            if (elements != null && elements.Any())
            {
                writer.WriteMapInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the required AsyncApi element map (string to string mapping).
        /// </summary>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The map values.</param>
        /// <param name="action">The map element writer action.</param>
        public static void WriteRequiredMap(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, string> elements,
            Action<IAsyncApiWriter, string> action)
        {
            writer.WriteMapInternal(name, elements, action);
        }

        public static void WriteRequiredMap<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, string, T> action)
            where T : IAsyncApiElement
        {
            if (elements != null && elements.Any())
            {
                writer.WriteMapInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the optional AsyncApi element map.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The map values.</param>
        /// <param name="action">The map element writer action with writer and value as input.</param>
        public static void WriteOptionalMap<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            if (elements != null && elements.Any())
            {
                writer.WriteMapInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the optional AsyncApi element map.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The map values.</param>
        /// <param name="action">The map element writer action with writer, key, and value as input.</param>
        public static void WriteOptionalMap<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, string, T> action)
            where T : IAsyncApiElement
        {
            if (elements != null && elements.Any())
            {
                writer.WriteMapInternal(name, elements, action);
            }
        }

        /// <summary>
        /// Write the required AsyncApi element map.
        /// </summary>
        /// <typeparam name="T">The AsyncApi element type. <see cref="IAsyncApiElement"/>.</typeparam>
        /// <param name="writer">The AsyncApi writer.</param>
        /// <param name="name">The property name.</param>
        /// <param name="elements">The map values.</param>
        /// <param name="action">The map element writer action.</param>
        public static void WriteRequiredMap<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, T> action)
            where T : IAsyncApiElement
        {
            writer.WriteMapInternal(name, elements, action);
        }

        private static void WriteCollectionInternal<T>(
            this IAsyncApiWriter writer,
            string name,
            IEnumerable<T> elements,
            Action<IAsyncApiWriter, T> action)
        {
            CheckArguments(writer, name, action);

            writer.WritePropertyName(name);
            writer.WriteStartArray();
            if (elements != null)
            {
                foreach (var item in elements)
                {
                    if (item != null)
                    {
                        action(writer, item);
                    }
                    else
                    {
                        writer.WriteNull();
                    }
                }
            }

            writer.WriteEndArray();
        }

        private static void WriteMapInternal<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, T> action)
        {
            WriteMapInternal(writer, name, elements, (w, k, s) => action(w, s));
        }

        private static void WriteMapInternal<T>(
            this IAsyncApiWriter writer,
            string name,
            IDictionary<string, T> elements,
            Action<IAsyncApiWriter, string, T> action)
        {
            CheckArguments(writer, name, action);

            writer.WritePropertyName(name);
            writer.WriteStartObject();

            if (elements != null)
            {
                foreach (var item in elements)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value != null)
                    {
                        action(writer, item.Key, item.Value);
                    }
                    else
                    {
                        writer.WriteNull();
                    }
                }
            }

            writer.WriteEndObject();
        }

        private static void CheckArguments<T>(IAsyncApiWriter writer, string name, Action<IAsyncApiWriter, T> action)
        {
            CheckArguments(writer, name);
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }
        }

        private static void CheckArguments<T>(IAsyncApiWriter writer, string name, Action<IAsyncApiWriter, string, T> action)
        {
            CheckArguments(writer, name);

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
        }

        private static void CheckArguments(IAsyncApiWriter writer, string name)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
        }
    }
}
