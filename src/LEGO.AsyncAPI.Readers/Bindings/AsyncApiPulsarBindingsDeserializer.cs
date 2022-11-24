// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models.Bindings.Pulsar;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<PulsarServerBinding> pulsarServerBindingFixedFields = new ()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "tenant", (a, n) => { a.Tenant = n.GetScalarValue(); } },
        };

        private static FixedFieldMap<PulsarChannelBinding> pulsarChannelBindingFixedFields = new ()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "namespace", (a, n) => { a.Namespace = n.GetScalarValue(); } },
            { "persistence", (a, n) => { a.Persistence = n.GetScalarValue(); } },
            { "compaction", (a, n) => { a.Compaction = n.GetIntegerValue(); } },
            { "retention", (a, n) => { a.Retention = LoadRetention(n); } },
            { "geo-replication", (a, n) => { a.GeoReplication = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "ttl", (a, n) => { a.TTL = n.GetIntegerValue(); } },
            { "deduplication", (a, n) => { a.Deduplication = n.GetBooleanValue(); } },
        };

        private static FixedFieldMap<RetentionDefinition> pulsarServerBindingRetentionFixedFields = new ()
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
