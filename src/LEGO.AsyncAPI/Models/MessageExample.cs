// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Any;

    /// <summary>
    /// Represents an example of a Message Object and MUST contain either headers and/or payload fields.
    /// </summary>
    public class MessageExample : IExtensible
    {
        /// <summary>
        /// The value of this field MUST validate against the Message Object's headers field.
        /// </summary>
        public IDictionary<string, IAny> Headers { get; set; } = new Dictionary<string, IAny>();

        /// <summary>
        /// The value of this field MUST validate against the Message Object's payload field.
        /// </summary>
        public IAny Payload { get; set; }

        /// <summary>
        /// A machine-friendly name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short summary of what the example is about.
        /// </summary>
        public string Summary { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> Extensions { get; set; } = new Dictionary<string, string>();
    }
}