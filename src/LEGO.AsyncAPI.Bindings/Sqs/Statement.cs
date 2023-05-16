namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Statement : IAsyncApiElement
    {

        public Effect Effect { get; set; }

        /// <summary>
        /// The AWS account or resource ARN that this statement applies to.
        /// </summary>
        // public StringOrStringList Principal { get; set; }
        public StringOrStringList Principal { get; set; }

        /// <summary>
        /// The SNS permission being allowed or denied e.g. sns:Publish
        /// </summary>
        public StringOrStringList Action { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("effect", this.Effect.GetDisplayName());
            writer.WriteRequiredObject("principal", this.Principal, (w, t) => t.Serialize(w));
            writer.WriteRequiredObject("action", this.Action, (w, t) => t.Serialize(w));
            writer.WriteEndObject();
        }
    }

    public enum Effect
    {
        [Display("allow")]
        Allow,
        [Display("deny")]
        Deny,
    }
}