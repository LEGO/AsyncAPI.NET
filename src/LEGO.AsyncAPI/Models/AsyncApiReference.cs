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
        /// Gets or sets external resource in the reference.
        /// It maybe:
        /// 1. a absolute/relative file path, for example:  ../commons/pet.json
        /// 2. a Url, for example: http://localhost/pet.json
        /// </summary>
        public string ExternalResource { get; set; }

        /// <summary>
        /// Gets or sets the element type referenced.
        /// </summary>
        /// <remarks>This must be present if <see cref="ExternalResource"/> is not present.</remarks>
        public ReferenceType? Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the reusable component of one particular ReferenceType.
        /// If ExternalResource is present, this is the path to the component after the '#/'.
        /// For example, if the reference is 'example.json#/path/to/component', the Id is 'path/to/component'.
        /// If ExternalResource is not present, this is the name of the component without the reference type name.
        /// For example, if the reference is '#/components/schemas/componentName', the Id is 'componentName'.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets a flag indicating whether this reference is an external reference.
        /// </summary>
        public bool IsExternal => this.ExternalResource != null;

        /// <summary>
        /// Gets a value indicating whether gets a flag indicating whether this reference is a local reference.
        /// </summary>
        public bool IsLocal => this.ExternalResource == null;

        /// <summary>
        /// Gets or sets the AsyncApiDocument that is hosting the AsyncApiReference instance. This is used to enable dereferencing the reference.
        /// </summary>
        public AsyncApiDocument HostDocument { get; set; } = null;

        /// <summary>
        /// Gets the full reference string for v3.0.
        /// </summary>
        public string Reference
        {
            get
            {
                if (this.IsExternal)
                {
                    return this.GetExternalReference();
                }

                if (!this.Type.HasValue)
                {
                    throw new ArgumentNullException(nameof(this.Type));
                }

                if (this.Type == ReferenceType.Tag)
                {
                    return this.Id;
                }

                if (this.Type == ReferenceType.SecurityScheme)
                {
                    return this.Id;
                }

                return "#/components/" + this.Type.GetDisplayName() + "/" + this.Id;
            }
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiReference"/> to Open Api v3.0.
        /// </summary>
        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Type == ReferenceType.Tag)
            {
                // Write the string value only
                writer.WriteValue(this.Reference);
                return;
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


        private string GetExternalReference()
        {
            if (this.Id != null)
            {
                    return this.ExternalResource + "#/components/" + this.Type.GetDisplayName() + "/"+ this.Id;
            }

            return this.ExternalResource;
        }
    }
}