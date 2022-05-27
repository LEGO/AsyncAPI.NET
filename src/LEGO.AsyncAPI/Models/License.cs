// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class License : IAsyncApiExtensible
    {
        public License(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets rEQUIRED. The license name used for the API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public Uri Url { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as License);
        }

        public bool Equals(License license)
        {
            return license != null &&
                   string.Compare(this.Name, license.Name, StringComparison.Ordinal) == 0 &&
                   ((this.Url == null && license.Url == null) || (this.Url != null && this.Url.Equals(license.Url)));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Name, this.Url);
        }
    }
}