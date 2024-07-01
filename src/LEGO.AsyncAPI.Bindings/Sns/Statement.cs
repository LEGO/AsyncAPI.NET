// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Statement : IAsyncApiExtensible
    {
        /// <summary>
        /// Indicates whether the policy allows or denies access.
        /// </summary>
        public Effect Effect { get; set; }

        /// <summary>
        /// The AWS account(s) or resource ARN(s) that this statement applies to.
        /// </summary>
        public AsyncApiAny Principal { get; set; }

        /// <summary>
        /// The SNS permission being allowed or denied e.g. sns:Publish
        /// </summary>
        public StringOrStringList Action { get; set; }

        /// <summary>
        /// The resource(s) that this policy applies to.
        /// </summary>
        public StringOrStringList? Resource { get; set; }

        /// <summary>
        /// Specific circumstances under which the policy grants permission.
        /// </summary>
        public AsyncApiAny? Condition { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("effect", this.Effect.GetDisplayName());
            writer.WriteRequiredObject("principal", this.Principal, (w, t) => t.Write(w));
            writer.WriteRequiredObject("action", this.Action, (w, t) => t.Value.Write(w));
            writer.WriteOptionalObject("resource", this.Resource, (w, t) => t?.Value.Write(w));
            writer.WriteOptionalObject("condition", this.Condition, (w, t) => t?.Write(w));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public enum Effect
    {
        [Display("Allow")]
        Allow,
        [Display("Deny")]
        Deny,
    }
}