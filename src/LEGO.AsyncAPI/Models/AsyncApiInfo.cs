// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The object provides metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    public class AsyncApiInfo : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// REQUIRED. The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// REQUIRED Provides the version of the application API (not to be confused with the specification version).
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// A short description of the application. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API. This MUST be in the form of an absolute URL.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        public AsyncApiContact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        public AsyncApiLicense License { get; set; }

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

            writer.WriteStartObject();

            // title
            writer.WriteRequiredProperty(AsyncApiConstants.Title, this.Title);

            // version
            writer.WriteOptionalProperty(AsyncApiConstants.Version, this.Version);

            // description
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            // termsOfService
            writer.WriteOptionalProperty(AsyncApiConstants.TermsOfService, this.TermsOfService?.OriginalString);

            // contact object
            writer.WriteOptionalObject(AsyncApiConstants.Contact, this.Contact, (w, c) => c.SerializeV2(w));

            // license object
            writer.WriteOptionalObject(AsyncApiConstants.License, this.License, (w, l) => l.SerializeV2(w));

            // specification extensions
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }

        public void SerializeV3(IAsyncApiWriter writer)
        {

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // title
            writer.WriteRequiredProperty(AsyncApiConstants.Title, this.Title);

            // version
            writer.WriteOptionalProperty(AsyncApiConstants.Version, this.Version);

            // description
            writer.WriteOptionalProperty(AsyncApiConstants.Description, this.Description);

            // termsOfService
            writer.WriteOptionalProperty(AsyncApiConstants.TermsOfService, this.TermsOfService?.OriginalString);

            // contact object
            writer.WriteOptionalObject(AsyncApiConstants.Contact, this.Contact, (w, c) => c.SerializeV3(w));

            // license object
            writer.WriteOptionalObject(AsyncApiConstants.License, this.License, (w, l) => l.SerializeV3(w));

            // tags
            writer.WriteOptionalCollection(AsyncApiConstants.Tags, this.Tags, (w, t) => t.SerializeV3(w));

            // external docs
            writer.WriteOptionalObject(AsyncApiConstants.ExternalDocs, this.ExternalDocs, (w, e) => e.SerializeV3(w));

            // specification extensions
            writer.WriteExtensions(this.Extensions);

            writer.WriteEndObject();
        }
    }
}