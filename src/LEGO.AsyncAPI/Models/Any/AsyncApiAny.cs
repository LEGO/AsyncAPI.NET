// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
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
        private JsonNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiAny"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public AsyncApiAny(JsonNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public JsonNode Node => this.node;

        public T GetValue<T>()
        {
            return this.node.GetValue<T>();
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
