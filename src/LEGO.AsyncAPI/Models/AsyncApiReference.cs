// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// A simple object to allow referencing other components in the specification, internally and externally.
    /// </summary>
    public class AsyncApiReference : IAsyncApiSerializable
    {
        /// <summary>
        /// Gets or sets the element type referenced.
        /// </summary>
        public ReferenceType? Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the reusable component of one particular ReferenceType.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the AsyncApiDocument that is hosting the AsyncApiReference instance. This is used to enable dereferencing the reference.
        /// </summary>
        public AsyncApiDocument HostDocument { get; set; } = null;

        /// <summary>
        /// Gets the full reference string for v2.3.
        /// </summary>
        public string Reference
        {
            get
            {
                if (!this.Type.HasValue)
                {
                    throw new ArgumentNullException(nameof(this.Type));
                }

                return "#/components/" + this.Type.GetDisplayName() + "/" + this.Id;
            }
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiReference"/> to Open Api v3.0.
        /// </summary>
        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Type == ReferenceType.SecurityScheme)
            {
                // Write the string as property name
                writer.WritePropertyName(this.Reference);
                return;
            }

            writer.WriteStartObject();

            // $ref
            writer.WriteProperty(AsyncApiConstants.DollarRef, this.Reference);

            writer.WriteEndObject();
        }
    }
}