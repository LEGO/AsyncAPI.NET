// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Converters;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a trait that MAY be applied to an Operation Object.
    /// </summary>
    public class AsyncApiOperationTrait : IAsyncApiExtensible, IAsyncApiReferenceable, IAsyncApiSerializable
    {
        /// <summary>
        /// unique string used to identify the operation.
        /// </summary>
        /// <remarks>
        /// The id MUST be unique among all operations described in the API.
        /// </remarks>
        public string OperationId { get; set; }

        /// <summary>
        /// a short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// a short summary of what the operation is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// a list of tags for API documentation control. Tags can be used for logical grouping of operations.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// additional external documentation for this operation.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
        /// </summary>
        public IDictionary<string, IOperationBinding> Bindings { get; set; } = new Dictionary<string, IOperationBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        /// <inheritdoc/>
        public bool UnresolvedReference { get; set; }

        /// <inheritdoc/>
        public AsyncApiReference Reference { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineAllReferences)
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.SerializeV2WithoutReference(writer);
        }

        public void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteProperty(AsyncApiConstants.OperationId, this.OperationId);
            writer.WriteProperty(AsyncApiConstants.Summary, this.Summary);
            writer.WriteProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));

            // TODO: Figure out bindings

            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}