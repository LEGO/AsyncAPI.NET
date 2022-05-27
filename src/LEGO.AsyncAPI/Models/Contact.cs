// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public class Contact : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets the identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pointing to the contact information. MUST be in the format of a URL.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        public string Email { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();
    }
}