// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Any;
    using Newtonsoft.Json;

    /// <summary>
    /// Describes a message received on a given channel and operation.
    /// </summary>
    public class Message : IExtensible, IReferenceable
    {
        /// <summary>
        /// Schema definition of the application headers. Schema MUST be of type "object".
        /// </summary>
        [JsonProperty("headers")]
        public Schema Headers { get; set; }

        /// <summary>
        /// Definition of the message payload. It can be of any type but defaults to Schema object. It must match the schema format, including encoding type - e.g Avro should be inlined as either a YAML or JSON object NOT a string to be parsed as YAML or JSON.
        /// </summary>
        [JsonProperty("payload")]
        // [JsonConverter(typeof(PayloadConverter))]
        public JToken Payload { get; set; }

        /// <summary>
        /// Definition of the correlation ID used for message tracing or matching.
        /// </summary>
        [JsonProperty("correlationId")]
        public CorrelationId CorrelationId { get; set; }

        /// <summary>
        /// A string containing the name of the schema format used to define the message payload.
        /// </summary>
        /// <remarks>
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </remarks>
        [JsonProperty("schemaFormat")]
        public string SchemaFormat { get; set; }

        /// <summary>
        /// The content type to use when encoding/decoding a message's payload.
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// A machine-friendly name for the message.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        [JsonProperty("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        [JsonProperty("tags")]
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// Additional external documentation for this message.
        /// </summary>
        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        [JsonProperty("bindings")]
        public IDictionary<string, IMessageBinding> Bindings { get; set; }

        /// <summary>
        /// List of examples.
        /// </summary>
        [JsonProperty("examples")]
        public IList<MessageExample> Examples { get; set; }

        /// <summary>
        /// A list of traits to apply to the message object. Traits MUST be merged into the message object using the JSON Merge Patch algorithm in the same order they are defined here. The resulting object MUST be a valid Message Object.
        /// </summary>
        [JsonProperty("traits")]
        public List<MessageTrait> Traits { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, JToken> Extensions { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public bool? UnresolvedReference { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public Reference Reference { get; set; }
    }
}