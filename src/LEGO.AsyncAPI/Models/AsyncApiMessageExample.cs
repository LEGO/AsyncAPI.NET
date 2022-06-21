// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Represents an example of a Message Object and MUST contain either headers and/or payload fields.
    /// </summary>
    public class AsyncApiMessageExample : IAsyncApiExtensible, IAsyncApiSerializable
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
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteOptionalMap(AsyncApiConstants.Headers, this.Headers, (w, h) => w.WriteAny(h));
            writer.WriteOptionalObject(AsyncApiConstants.Payload, this.Payload, (w, p) => w.WriteAny(p));
        }
    }
}