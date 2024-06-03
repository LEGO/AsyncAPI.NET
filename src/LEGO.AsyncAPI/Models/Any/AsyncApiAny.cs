﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// AsyncApiAny.
    /// </summary>
    /// <seealso cref="LEGO.AsyncAPI.Models.Interfaces.IAsyncApiElement" />
    /// <seealso cref="LEGO.AsyncAPI.Models.Interfaces.IAsyncApiExtension" />
    public class AsyncApiAny : IAsyncApiElement, IAsyncApiExtension
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private JsonNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiAny" /> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public AsyncApiAny(JsonNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiAny"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public AsyncApiAny(object obj)
        {
            this.node = JsonNode.Parse(JsonSerializer.Serialize(obj, this.options));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiAny"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public AsyncApiAny(JsonArray node)
        {
            this.node = node;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiAny"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public AsyncApiAny(JsonObject node)
        {
            this.node = node;
        }

        /// <summary>
        /// Converts to <see cref="{T}" /> from an Extension.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="extension">The extension.</param>
        /// <returns><see cref="{T}"/>.</returns>
        public static T FromExtensionOrDefault<T>(IAsyncApiExtension extension)
        {
            if (extension is AsyncApiAny any)
            {
                return any.GetValueOrDefault<T>();
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <returns><see cref="JsonNode"/>.</returns>
        /// <value>
        /// The node.
        /// </value>
        public JsonNode GetNode() => this.node;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"><see cref="{T}" />.</typeparam>
        /// <returns><see cref="{T}" />.</returns>
        public T GetValue<T>()
        {
            if (this.node == null)
            {
                return default(T);
            }

            if (this.node is JsonValue)
            {
                return this.node.GetValue<T>();
            }

            return JsonSerializer.Deserialize<T>(this.node.ToJsonString());
        }

        /// <summary>
        /// Gets the value or default.
        /// </summary>
        /// <typeparam name="T"><see cref="{T}" />.</typeparam>
        /// <returns><see cref="{T}" /> or default.</returns>
        public T GetValueOrDefault<T>()
        {
            try
            {
                return this.GetValue<T>();
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <typeparam name="T"><see cref="{T}" />.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>true if the value could be converted, otherwise false.</returns>
        public bool TryGetValue<T>(out T value)
        {
            try
            {
                value = this.GetValue<T>();
                return true;
            }
            catch (System.Exception)
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Writes the Any type.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(IAsyncApiWriter writer)
        {
            writer.WriteAny(new AsyncApiAny(this.node));
        }
    }
}
