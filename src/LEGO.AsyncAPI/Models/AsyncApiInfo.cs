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
        /// Gets or sets REQUIRED. The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets REQUIRED. Provides the version of the application API (not to be confused with the specification version).
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a short description of the application. CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a URL to the Terms of Service for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the exposed API.
        /// </summary>
        public AsyncApiContact Contact { get; set; }

        /// <summary>
        /// Gets or sets the license information for the exposed API.
        /// </summary>
        public AsyncApiLicense License { get; set; }

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
            writer.WriteOptionalProperty(AsyncApiConstants.Title, this.Title);

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
    }
}