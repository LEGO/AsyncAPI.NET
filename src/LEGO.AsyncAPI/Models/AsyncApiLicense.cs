// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class AsyncApiLicense : IAsyncApiSerializable, IAsyncApiExtensible
    {
        public AsyncApiLicense(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets REQUIRED. The license name used for the API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

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
            writer.WriteProperty(AsyncApiConstants.Name, this.Name);

            // url
            writer.WriteProperty(AsyncApiConstants.Url, this.Url?.OriginalString);

            // specification extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_2_0);

            writer.WriteEndObject();
        }
    }
}