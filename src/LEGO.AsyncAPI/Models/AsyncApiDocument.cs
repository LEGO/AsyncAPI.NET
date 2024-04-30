// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using LEGO.AsyncAPI.Services;

    /// <summary>
    /// This is the root document object for the API specification. It combines resource listing and API declaration together into one document.
    /// </summary>
    public class AsyncApiDocument : IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// REQUIRED. Specifies the AsyncAPI Specification version being used.
        /// </summary>
        public string Asyncapi { get; set; }

        /// <summary>
        /// Identifier of the application the AsyncAPI document is defining.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// REQUIRED. Provides metadata about the API. The metadata can be used by the clients if needed.
        /// </summary>
        public AsyncApiInfo Info { get; set; }

        /// <summary>
        /// provides connection details of servers.
        /// </summary>
        public IDictionary<string, AsyncApiServer> Servers { get; set; } = new Dictionary<string, AsyncApiServer>();

        /// <summary>
        /// default content type to use when encoding/decoding a message's payload.
        /// </summary>
        /// <remarks>
        /// A string representing the default content type to use when encoding/decoding a message's payload.
        /// The value MUST be a specific media type (e.g. application/json). This value MUST be used by schema parsers when the contentType property is omitted.
        /// </remarks>
        public string DefaultContentType { get; set; }

        /// <summary>
        /// REQUIRED. The available channels and messages for the API.
        /// </summary>
        public IDictionary<string, AsyncApiChannel> Channels { get; set; } = new Dictionary<string, AsyncApiChannel>();

        /// <summary>
        /// an element to hold various schemas for the specification.
        /// </summary>
        public AsyncApiComponents Components { get; set; }

        /// <summary>
        /// a list of tags used by the specification with additional metadata. Each tag name in the list MUST be unique.
        /// </summary>
        public IList<AsyncApiTag> Tags { get; set; } = new List<AsyncApiTag>();

        /// <summary>
        /// additional external documentation.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (writer.GetSettings().InlineReferences)
            {
                this.ResolveReferences();
            }

            writer.WriteStartObject();

            // asyncApi
            writer.WriteRequiredProperty(AsyncApiConstants.AsyncApi, "2.6.0");

            // info
            writer.WriteRequiredObject(AsyncApiConstants.Info, this.Info, (w, i) => i.SerializeV2(w));

            // id
            writer.WriteOptionalProperty(AsyncApiConstants.Id, this.Id);

            // servers
            writer.WriteOptionalMap(AsyncApiConstants.Servers, this.Servers, (writer, key, component) =>
            {
                if (component.Reference != null &&
                component.Reference.Type == ReferenceType.Server &&
                component.Reference.Id == key)
                {
                    component.SerializeV2WithoutReference(writer);
                }
                else
                {
                    component.SerializeV2(writer);
                }
            });

            // content type
            writer.WriteOptionalProperty(AsyncApiConstants.DefaultContentType, this.DefaultContentType);

            // channels
            writer.WriteRequiredMap(AsyncApiConstants.Channels, this.Channels, (writer, key, component) =>
            {
                if (component.Reference != null &&
                component.Reference.Type == ReferenceType.Channel &&
                component.Reference.Id == key)
                {
                    component.SerializeV2WithoutReference(writer);
                }
                else
                {
                    component.SerializeV2(writer);
                }
            });

            // components
            writer.WriteOptionalObject(AsyncApiConstants.Components, this.Components, (w, c) => c.SerializeV2(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV2(w));

            // external docs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV2(w));

            // extensions
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        public IEnumerable<AsyncApiError> ResolveReferences()
        {
            var resolver = new AsyncApiReferenceResolver(this);
            var walker = new AsyncApiWalker(resolver);
            walker.Walk(this);
            return resolver.Errors;
        }

        internal T ResolveReference<T>(AsyncApiReference reference)
            where T : class, IAsyncApiReferenceable
        {
            return this.ResolveReference(reference) as T;
        }

        internal IAsyncApiReferenceable ResolveReference(AsyncApiReference reference)
        {
            if (reference == null)
            {
                return null;
            }

            if (!reference.Type.HasValue)
            {
                throw new ArgumentException("Reference must have a type.");
            }

            if (this.Components == null)
            {
                throw new AsyncApiException(string.Format("Invalid reference Id: '{0}'", reference.Id));
            }

            try
            {
                switch (reference.Type)
                {
                    case ReferenceType.Schema:
                        return this.Components.Schemas[reference.Id];
                    case ReferenceType.Server:
                        return this.Components.Servers[reference.Id];
                    case ReferenceType.Channel:
                        return this.Components.Channels[reference.Id];
                    case ReferenceType.Message:
                        return this.Components.Messages[reference.Id];
                    case ReferenceType.SecurityScheme:
                        return this.Components.SecuritySchemes[reference.Id];
                    case ReferenceType.Parameter:
                        return this.Components.Parameters[reference.Id];
                    case ReferenceType.CorrelationId:
                        return this.Components.CorrelationIds[reference.Id];
                    case ReferenceType.OperationTrait:
                        return this.Components.OperationTraits[reference.Id];
                    case ReferenceType.MessageTrait:
                        return this.Components.MessageTraits[reference.Id];
                    case ReferenceType.ServerBindings:
                        return this.Components.ServerBindings[reference.Id];
                    case ReferenceType.ChannelBindings:
                        return this.Components.ChannelBindings[reference.Id];
                    case ReferenceType.OperationBindings:
                        return this.Components.OperationBindings[reference.Id];
                    case ReferenceType.MessageBindings:
                        return this.Components.MessageBindings[reference.Id];
                    default:
                        throw new AsyncApiException("Invalid reference type.");
                }
            }
            catch (KeyNotFoundException)
            {
                throw new AsyncApiException(string.Format("Invalid reference Id: '{0}'", reference.Id));
            }
        }
    }
}
