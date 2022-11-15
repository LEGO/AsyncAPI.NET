// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Pulsar;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<PulsarServerBinding> pulsarServerBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "retention", (a, n) => { a.Retention = LoadRetention(n); } },
            { "deduplication", (a, n) => { a.Deduplication = n.GetBooleanValue(); } },
        };

        private static FixedFieldMap<PulsarChannelBinding> pulsarChannelBindingFixedFields = new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "persistence", (a, n) => { a.Persistence = n.GetScalarValue(); } },
            { "compaction", (a, n) => { a.Compaction = n.GetLongValue(); } },
            { "retention", (a, n) => { a.Retention = LoadRetention(n); } },
            { "deduplication", (a, n) => { a.Deduplication = n.GetBooleanValue(); } },
        };

        private static FixedFieldMap<RetentionDefinition> pulsarServerBindingRetentionFixedFields = new()
        {
            { "time", (a, n) => { a.Time = n.GetIntegerValue(); } },
            { "size", (a, n) => { a.Size = n.GetIntegerValue(); } },
        };

        private static RetentionDefinition LoadRetention(ParseNode node)
        {
            var mapNode = node.CheckMapNode("retention");
            var retention = new RetentionDefinition();
            ParseMap(mapNode, retention, pulsarServerBindingRetentionFixedFields, null);
            return retention;
        }
    }
}