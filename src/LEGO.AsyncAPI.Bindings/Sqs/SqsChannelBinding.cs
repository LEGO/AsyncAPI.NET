using System;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Sqs
{
    /// <summary>
    /// This object contains information about the channel representation in SQS.
    /// </summary>
    public class SqsChannelBinding : ChannelBinding<SqsChannelBinding>
    {
        /// <summary>
        /// A definition of the queue that will be used as the channel.
        /// </summary>
        public Queue Queue { get; set; }

        /// <summary>
        /// A definition of the queue that will be used for un-processable messages.
        /// </summary>
        public Queue DeadLetterQueue { get; set; }

        public override string BindingKey => "sqs";

        public override void SerializeProperties(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredObject("queue", this.Queue, (w, q) => q.Serialize(w));
            writer.WriteOptionalObject("deadLetterQueue", this.DeadLetterQueue, (w, q) => q.Serialize(w));
            writer.WriteEndObject();
        }

        protected override FixedFieldMap<SqsChannelBinding> FixedFieldMap => new()
        {
            
        };
    }
}