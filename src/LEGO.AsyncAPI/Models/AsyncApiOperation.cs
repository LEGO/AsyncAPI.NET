// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes a publish or a subscribe operation. This provides a place to document how and why messages are sent and received.
    /// </summary>
    public class AsyncApiOperation : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// unique string used to identify the operation.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// a short summary of what the operation is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// a verbose explanation of the operation. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used with this server. The list of values includes alternative security requirement objects that can be used. Only one of the security requirement objects need to be satisfied to authorize a connection or operation
        /// </summary>
        public IList<AsyncApiSecurityRequirement> Security { get; set; } = new List<AsyncApiSecurityRequirement>();

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
        public AsyncApiBindings<IOperationBinding> Bindings { get; set; } = new AsyncApiBindings<IOperationBinding>();

        /// <summary>
        /// a list of traits to apply to the operation object.
        /// </summary>
        public IList<AsyncApiOperationTrait> Traits { get; set; } = new List<AsyncApiOperationTrait>();

        /// <summary>
        /// a definition of the message that will be published or received on this channel.
        /// </summary>
        /// <remarks>
        /// `oneOf` is allowed here to specify multiple messages, however, a message MUST be valid only against one of the referenced message objects.
        /// </remarks>
        public IList<AsyncApiMessage> Message { get; set; } = new List<AsyncApiMessage>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty(AsyncApiConstants.OperationId, this.OperationId);
            writer.WriteOptionalProperty(AsyncApiConstants.Summary, this.Summary);
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalCollection(AsyncApiConstants.Security, this.Security, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));

            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalCollection(AsyncApiConstants.Traits, this.Traits, (w, t) => t.SerializeV2(w));
            if (this.Message.Count > 1)
            {
                writer.WritePropertyName(AsyncApiConstants.Message);
                writer.WriteStartObject();
                writer.WriteOptionalCollection(AsyncApiConstants.OneOf, this.Message, (w, t) => t.SerializeV2(w));
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteOptionalObject(AsyncApiConstants.Message, this.Message.FirstOrDefault(), (w, m) => m.SerializeV2(w));
            }

            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}