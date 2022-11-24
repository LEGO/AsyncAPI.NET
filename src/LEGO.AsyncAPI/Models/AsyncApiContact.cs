// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public class AsyncApiContact : IAsyncApiSerializable, IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets the identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pointing to the contact information. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        public string Email { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            // name
            writer.WriteOptionalProperty(AsyncApiConstants.Name, this.Name);

            // url
            writer.WriteOptionalProperty(AsyncApiConstants.Url, this.Url?.OriginalString);

            // email
            writer.WriteOptionalProperty(AsyncApiConstants.Email, this.Email);

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}