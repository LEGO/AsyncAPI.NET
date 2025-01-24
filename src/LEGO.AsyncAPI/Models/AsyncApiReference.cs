// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// A simple object to allow referencing other components in the specification, internally and externally.
    /// </summary>
    public class AsyncApiReference : IAsyncApiSerializable
    {
        public AsyncApiReference()
        {
        }

        public AsyncApiReference(string reference, ReferenceType? type)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new AsyncApiException($"The reference string '{reference}' has invalid format.");
            }

            var segments = reference.Split('#');
            if (segments.Length == 1)
            {
                if (type == ReferenceType.SecurityScheme)
                {
                    this.Type = type;
                    this.Id = reference;
                    return;
                }

                this.Type = type;
                this.ExternalResource = segments[0];

                return;
            }
            else if (segments.Length == 2)
            {
                // Local components reference
                if (reference.StartsWith("#"))
                {
                    var localSegments = reference.Split('/');

                    if (localSegments.Length == 4)
                    {
                        if (localSegments[1] == "components")
                        {
                            var referenceType = localSegments[2].GetEnumFromDisplayName<ReferenceType>();

                            this.Type = referenceType;
                            this.Id = localSegments[3];
                            this.IsFragment = true;
                            return;
                        }
                    }

                    throw new AsyncApiException($"The reference string '{reference}' has invalid format.");
                }

                var id = segments[1];
                if (id.StartsWith("/components/"))
                {
                    var localSegments = segments[1].Split('/');
                    var referencedType = localSegments[2].GetEnumFromDisplayName<ReferenceType>();
                    if (type == null)
                    {
                        type = referencedType;
                    }
                    else
                    {
                        if (type != referencedType)
                        {
                            throw new AsyncApiException("Referenced type mismatch");
                        }
                    }

                    id = localSegments[3];
                }

                this.IsFragment = true;
                this.ExternalResource = segments[0];
                this.Type = type;
                this.Id = id;

                return;
            }

            throw new AsyncApiException($"The reference string '{reference}' has invalid format.");
        }

        /// <summary>
        /// External resource in the reference.
        /// It maybe:
        /// 1. a absolute/relative file path, for example:  ../commons/pet.json
        /// 2. a Url, for example: http://localhost/pet.json.
        /// </summary>
        public string ExternalResource
        {
            get;
            set;
        }

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
        /// Gets a flag indicating whether a file is a valid OpenAPI document or a fragment.
        /// </summary>
        public bool IsFragment { get; set; } = false;

        /// <summary>
        /// Gets a flag indicating whether this reference is an external reference.
        /// </summary>
        public bool IsExternal => this.ExternalResource != null;

        /// <summary>
        /// Gets the full reference string for v2.
        /// </summary>
        public string Reference
        {
            get
            {
                if (this.IsExternal)
                {
                    return this.GetExternalReferenceV2();
                }

                if (!this.Type.HasValue)
                {
                    throw new ArgumentNullException(nameof(this.Type));
                }

                //if (this.Type == ReferenceType.SecurityScheme)
                //{
                //    return this.Id;
                //}

                return "#/components/" + this.Type.GetDisplayName() + "/" + this.Id;
            }
        }

        /// <summary>
        /// Serialize <see cref="AsyncApiReference"/> to Async Api.
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
            writer.WriteOptionalProperty(AsyncApiConstants.DollarRef, this.Reference);

            writer.WriteEndObject();
        }

        private string GetExternalReferenceV2()
        {

            return this.ExternalResource + (this.Id != null ? "/#/" + this.Id : string.Empty);
        }

        public void Write(IAsyncApiWriter writer)
        {
            this.SerializeV2(writer);
        }
    }
}