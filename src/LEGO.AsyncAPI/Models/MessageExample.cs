// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Represents an example of a Message Object and MUST contain either headers and/or payload fields.
    /// </summary>
    public class MessageExample : IAsyncApiExtensible
    {
        /// <summary>
        /// Gets or sets the value of this field MUST validate against the Message Object's headers field.
        /// </summary>
        public IDictionary<string, IAsyncApiAny> Headers { get; set; } = new Dictionary<string, IAsyncApiAny>();

        /// <summary>
        /// Gets or sets the value of this field MUST validate against the Message Object's payload field.
        /// </summary>
        public IAsyncApiAny Payload { get; set; }

        /// <summary>
        /// Gets or sets a machine-friendly name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a short summary of what the example is about.
        /// </summary>
        public string Summary { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, IAsyncApiAny> Extensions { get; set; } = new Dictionary<string, IAsyncApiAny>();
    }
}