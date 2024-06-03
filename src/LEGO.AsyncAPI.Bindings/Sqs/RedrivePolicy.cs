// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class RedrivePolicy : IAsyncApiExtensible
    {
        /// <summary>
        /// Prevent poison pill messages by moving un-processable messages to an SQS dead letter queue.
        /// </summary>
        public Identifier DeadLetterQueue { get; set; }

        /// <summary>
        /// The number of times a message is delivered to the source queue before being moved to the dead-letter queue.
        /// </summary>
        public int? MaxReceiveCount { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredObject("deadLetterQueue", this.DeadLetterQueue, (w, q) => q.Serialize(w));
            writer.WriteOptionalProperty("maxReceiveCount", this.MaxReceiveCount);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}