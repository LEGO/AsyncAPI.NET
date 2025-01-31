// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes a trait that MAY be applied to an Operation Object.
    /// </summary>
    public class AsyncApiOperationTrait : IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// unique string used to identify the operation.
        /// </summary>
        /// <remarks>
        /// The id MUST be unique among all operations described in the API.
        /// </remarks>
        public virtual string OperationId { get; set; }

        /// <summary>
        /// a short summary of what the operation is about.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// a short summary of what the operation is about.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// a list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public virtual IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// additional external documentation for this operation.
        /// </summary>
        public virtual AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
        /// </summary>
        public virtual AsyncApiBindings<IOperationBinding> Bindings { get; set; } = new AsyncApiBindings<IOperationBinding>();

        /// <inheritdoc/>
        public virtual IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public virtual void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteOptionalProperty(AsyncApiConstants.OperationId, this.OperationId);
            writer.WriteOptionalProperty(AsyncApiConstants.Summary, this.Summary);
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV2(w));
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}