﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Allows adding meta data to a single tag.
    /// </summary>
    public class AsyncApiTag : IAsyncApiExtensible, IAsyncApiSerializable, IAsyncApiReferenceable
    {
        /// <summary>
        /// REQUIRED. The name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// a short description for the tag. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// additional external documentation for this tag.
        /// </summary>
        public AsyncApiExternalDocumentation ExternalDocs { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Name, this.Name);

            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV3(w));

            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        public void SerializeV3(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV3(writer);
                return;
            }

            writer.WriteStartObject();

            writer.WriteRequiredProperty(AsyncApiConstants.Name, this.Name);

            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV3(w));

            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}