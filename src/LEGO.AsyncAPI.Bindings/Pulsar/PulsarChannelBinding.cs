using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Bindings.Pulsar;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Pulsar
{
    public class PulsarChannelBinding : ChannelBinding<PulsarChannelBinding>
    { 
        /// <summary>
        /// The namespace associated with the topic.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// persistence of the topic in Pulsar persistent or non-persistent.
        /// </summary>
        public Persistence? Persistence { get; set; }

        /// <summary>
        /// Topic compaction threshold given in bytes.
        /// </summary>
        public int? Compaction { get; set; }

        /// <summary>
        /// A list of clusters the topic is replicated to.
        /// </summary>
        public IEnumerable<string> GeoReplication { get; set; }

        /// <summary>
        /// Topic retention policy.
        /// </summary>
        public RetentionDefinition Retention { get; set; }

        /// <summary>
        /// Message Time-to-live in seconds. 
        /// </summary>
        public int? TTL { get; set; }

        /// <summary>
        /// When Message deduplication is enabled, it ensures that each message produced on Pulsar topics is persisted to disk only once.
        /// </summary>
        public bool? Deduplication { get; set; }

        public override string Type => "pulsar";

        protected override FixedFieldMap<PulsarChannelBinding> FixedFieldMap => new()
        {
            { "bindingVersion", (a, n) => { a.BindingVersion = n.GetScalarValue(); } },
            { "namespace", (a, n) => { a.Namespace = n.GetScalarValue(); } },
            { "persistence", (a, n) => { a.Persistence = n.GetScalarValue().GetEnumFromDisplayName<Persistence>(); } },
            { "compaction", (a, n) => { a.Compaction = n.GetIntegerValue(); } },
            { "retention", (a, n) => { a.Retention = LoadRetention(n); } },
            { "geo-replication", (a, n) => { a.GeoReplication = n.CreateSimpleList(s => s.GetScalarValue()); } },
            { "ttl", (a, n) => { a.TTL = n.GetIntegerValue(); } },
            { "deduplication", (a, n) => { a.Deduplication = n.GetBooleanValue(); } },
        };

        private FixedFieldMap<RetentionDefinition> pulsarServerBindingRetentionFixedFields = new()
        {
            { "time", (a, n) => { a.Time = n.GetIntegerValue(); } },
            { "size", (a, n) => { a.Size = n.GetIntegerValue(); } },
        };

        private RetentionDefinition LoadRetention(ParseNode node)
        {
            var mapNode = node.CheckMapNode("retention");
            var retention = new RetentionDefinition();
            ParseMap(mapNode, retention, pulsarServerBindingRetentionFixedFields, null);
            return retention;
        }

        public override void SerializeV2WithoutReference(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty(AsyncApiConstants.Namespace, this.Namespace);
            writer.WriteRequiredProperty(AsyncApiConstants.Persistence, this.Persistence.GetDisplayName());
            writer.WriteOptionalProperty<int>(AsyncApiConstants.Compaction, this.Compaction);
            writer.WriteOptionalCollection(AsyncApiConstants.GeoReplication, this.GeoReplication, (v, s) => v.WriteValue(s));
            writer.WriteOptionalObject(AsyncApiConstants.Retention, this.Retention, (w, r) => r.Serialize(w));
            writer.WriteOptionalProperty<int>(AsyncApiConstants.TTL, this.TTL);
            writer.WriteOptionalProperty(AsyncApiConstants.Deduplication, this.Deduplication);
            writer.WriteOptionalProperty(AsyncApiConstants.BindingVersion, this.BindingVersion);

            writer.WriteEndObject();
        }
    }
}