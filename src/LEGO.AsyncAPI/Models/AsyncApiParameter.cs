// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Describes a parameter included in a channel name.
    /// </summary>
    public class AsyncApiParameter : IAsyncApiReferenceable, IAsyncApiExtensible, IAsyncApiSerializable
    {
        /// <summary>
        /// Gets or sets a verbose explanation of the parameter. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets definition of the parameter.
        /// </summary>
        public AsyncApiSchema Schema { get; set; }

        /// <summary>
        /// Gets or sets a runtime expression that specifies the location of the parameter value.
        /// </summary>
        public string Location { get; set; }

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

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
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
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);
            writer.WriteOptionalObject(AsyncApiConstants.Schema, this.Schema, (w, s) => s.SerializeV2(w));
            writer.WriteOptionalProperty(AsyncApiConstants.Location, this.Location);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}