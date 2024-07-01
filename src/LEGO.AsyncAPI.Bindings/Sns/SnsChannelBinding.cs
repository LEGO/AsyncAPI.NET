// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Bindings;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Binding class for SNS channel settings.
    /// </summary>
    public class SnsChannelBinding : ChannelBinding<SnsChannelBinding>
    {
        /// <summary>
        /// The name of the topic. Can be different from the channel name to allow flexibility around AWS resource naming limitations.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// By default, we assume an unordered SNS topic. This field allows configuration of a FIFO SNS Topic.
        /// </summary>
        public Ordering Ordering { get; set; }

        /// <summary>
        /// The security policy for the SNS Topic.
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// Key-value pairs that represent AWS tags on the topic.
        /// </summary>
        public Dictionary<string, string> Tags { get; set; }

        public override string BindingKey => "sns";

        protected override FixedFieldMap<SnsChannelBinding> FixedFieldMap => new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "ordering", (a, n) => { a.Ordering = n.ParseMapWithExtensions(this.orderingFixedFields); } },
            { "policy", (a, n) => { a.Policy = n.ParseMapWithExtensions(this.policyFixedFields); } },
            { "tags", (a, n) => { a.Tags = n.CreateSimpleMap(s => s.GetScalarValue()); } },
        };

        private FixedFieldMap<Ordering> orderingFixedFields = new()
        {
            { "type", (a, n) => { a.Type = n.GetScalarValue().GetEnumFromDisplayName<OrderingType>(); } },
            { "contentBasedDeduplication", (a, n) => { a.ContentBasedDeduplication = n.GetBooleanValue(); } },
        };

        private FixedFieldMap<Policy> policyFixedFields = new()
        {
            { "statements", (a, n) => { a.Statements = n.CreateList(s => s.ParseMapWithExtensions(statementFixedFields)); } },
        };

        private static FixedFieldMap<Statement> statementFixedFields = new()
        {
            { "effect", (a, n) => { a.Effect = n.GetScalarValue().GetEnumFromDisplayName<Effect>(); } },
            { "principal", (a, n) => { a.Principal = n.CreateAny(); } },
            { "action", (a, n) => { a.Action = StringOrStringList.Parse(n); } },
            { "resource", (a, n) => { a.Resource = StringOrStringList.Parse(n); } },
            { "condition", (a, n) => { a.Condition = n.CreateAny(); } },
        };

        /// <inheritdoc/>
        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("name", this.Name);
            writer.WriteOptionalObject("ordering", this.Ordering, (w, t) => t.Serialize(w));
            writer.WriteOptionalObject("policy", this.Policy, (w, t) => t.Serialize(w));
            writer.WriteOptionalMap("tags", this.Tags, (w, t) => w.WriteValue(t));
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}