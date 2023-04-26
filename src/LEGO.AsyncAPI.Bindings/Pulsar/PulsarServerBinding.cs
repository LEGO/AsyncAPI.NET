// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Pulsar
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for Pulsar server settings.
    /// </summary>
    public class PulsarServerBinding : ServerBinding<PulsarServerBinding>
    {
        /// <summary>
        /// The pulsar tenant. If omitted, "public" must be assumed.
        /// </summary>
        public string Tenant { get; set; }

        /// <summary>
        /// The version of this binding.
        public string BindingVersion { get; set; }

        public override string Type => "pulsar";

        protected override FixedFieldMap<PulsarServerBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "tenant", (a, n) => { a.Tenant = n.GetScalarValue(); } },
        };

        public override void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();

            writer.WriteOptionalProperty(AsyncApiConstants.Tenant, this.Tenant);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);

            writer.WriteEndObject();
        }
    }
}
