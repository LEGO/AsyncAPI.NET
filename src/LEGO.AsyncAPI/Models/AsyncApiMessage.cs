// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes a message received on a given channel and operation.
    /// </summary>
    public class AsyncApiMessage : IAsyncApiExtensible, IAsyncApiReferenceable, IAsyncApiSerializable
    {
        /// <summary>
        /// schema definition of the application headers. Schema MUST be of type "object".
        /// </summary>
        public AsyncApiSchema Headers { get; set; }

        /// <summary>
        /// definition of the message payload. It can be of any type but defaults to Schema object. It must match the schema format, including encoding type - e.g Avro should be inlined as either a YAML or JSON object NOT a string to be parsed as YAML or JSON.
        /// </summary>
        public IAsyncApiAny Payload { get; set; }

        /// <summary>
        /// definition of the correlation ID used for message tracing or matching.
        /// </summary>
        public AsyncApiCorrelationId CorrelationId { get; set; }

        /// <summary>
        /// a string containing the name of the schema format used to define the message payload.
        /// </summary>
        /// <remarks>
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </remarks>
        public string SchemaFormat { get; set; }

        /// <summary>
        /// the content type to use when encoding/decoding a message's payload.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// a machine-friendly name for the message.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// a human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// a short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// a verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// a list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// additional external documentation for this message.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        public IDictionary<string, IMessageBinding> Bindings { get; set; } = new Dictionary<string, IMessageBinding>();

        /// <summary>
        /// list of examples.
        /// </summary>
        public IList<MessageExample> Examples { get; set; } = new List<MessageExample>();

        /// <summary>
        /// a list of traits to apply to the message object. Traits MUST be merged into the message object using the JSON Merge Patch algorithm in the same order they are defined here. The resulting object MUST be a valid Message Object.
        /// </summary>
        public IList<MessageTrait> Traits { get; set; } = new List<MessageTrait>();

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

            //TODO MISSING

            writer.WriteStartObject();
            writer.WriteOptionalObject(AsyncApiConstants.Headers, this.Headers, (w, h) => h.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.Payload, Payload, (w, p) => w.WriteAny(p));
            writer.WriteOptionalObject(AsyncApiConstants.CorrelationId, CorrelationId, (w, c) => c.SerializeV2(w));
            writer.WriteProperty(AsyncApiConstants.SchemaFormat, this.SchemaFormat);
            writer.WriteProperty(AsyncApiConstants.ContentType, this.ContentType);
            writer.WriteProperty(AsyncApiConstants.Name, this.Name);
            writer.WriteProperty(AsyncApiConstants.Title, this.Title);
            writer.WriteProperty(AsyncApiConstants.Summary, this.Summary);
            writer.WriteProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));
            // TODO: figure out bindings

            writer.WriteOptionalCollection(AsyncApiConstants.Examples, this.Examples, (w, e) => e.SerializeV2(w));

            writer.WriteOptionalCollection(AsyncApiConstants.Traits, this.Traits, (w, t) => t.SerializeV2(w));
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);
            writer.WriteEndObject();
        }
    }
}