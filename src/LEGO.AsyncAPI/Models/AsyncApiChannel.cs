// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    public class AsyncApiChannel : IAsyncApiReferenceable, IAsyncApiExtensible
    {
        /// <summary>
        /// an optional description of this channel item. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// the servers on which this channel is available, specified as an optional unordered list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// </summary>
        /// <remarks>
        /// If servers is absent or empty then this channel must be available on all servers defined in the Servers Object.
        /// </remarks>
        public IList<string> Servers { get; set; } = new List<string>();

        /// <summary>
        /// a definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
        /// </summary>
        public AsyncApiOperation Subscribe { get; set; }

        /// <summary>
        /// a definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
        /// </summary>
        public AsyncApiOperation Publish { get; set; }

        /// <summary>
        /// a map of the parameters included in the channel name. It SHOULD be present only when using channels with expressions (as defined by RFC 6570 section 2.2).
        /// </summary>
        public IDictionary<string, AsyncApiParameter> Parameters { get; set; } = new Dictionary<string, AsyncApiParameter>();

        /// <summary>
        /// a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the channel.
        /// </summary>
        public AsyncApiBindings<IChannelBinding> Bindings { get; set; } = new AsyncApiBindings<IChannelBinding>();

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && writer.GetSettings().ReferenceInline != ReferenceInlineSetting.InlineReferences)
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

            // description
            writer.WriteProperty(AsyncApiConstants.Description, this.Description);

            // servers
            writer.WriteOptionalCollection(AsyncApiConstants.Servers, this.Servers, (w, s) => w.WriteValue(s));

            // subscribe
            writer.WriteOptionalObject(AsyncApiConstants.Subscribe, this.Subscribe, (w, s) => s.SerializeV2(w));

            // publish
            writer.WriteOptionalObject(AsyncApiConstants.Publish, this.Publish, (w, s) => s.SerializeV2(w));

            // parameters
            writer.WriteOptionalMap(AsyncApiConstants.Parameters, this.Parameters, (writer, key, component) =>
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

            writer.WriteOptionalObject(AsyncApiConstants.Bindings, this.Bindings, (w, t) => t.SerializeV2(w));

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}