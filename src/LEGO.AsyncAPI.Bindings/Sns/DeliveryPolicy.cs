namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;
    
    public class DeliveryPolicy : IAsyncApiExtensible
    {
        /// <summary>
        /// The minimum delay for a retry in seconds.
        /// </summary>
        public int? MinDelayTarget { get; set; }

        /// <summary>
        /// The maximum delay for a retry in seconds.
        /// </summary>
        public int? MaxDelayTarget { get; set; }

        /// <summary>
        /// The total number of retries, including immediate, pre-backoff, backoff, and post-backoff retries.
        /// </summary>
        public int? NumRetries { get; set; }

        /// <summary>
        /// The number of immediate retries (with no delay).
        /// </summary>
        public int? NumNoDelayRetries { get; set; }

        /// <summary>
        /// The number of immediate retries (with delay).
        /// </summary>
        public int? NumMinDelayRetries { get; set; }

        /// <summary>
        /// The number of post-backoff phase retries, with the maximum delay between retries.
        /// </summary>
        public int? NumMaxDelayRetries { get; set; }

        /// <summary>
        /// The algorithm for backoff between retries.
        /// </summary>
        public BackoffFunction BackoffFunction { get; set; }

        /// <summary>
        /// The maximum number of deliveries per second, per subscription.
        /// </summary>
        public int? MaxReceivesPerSecond { get; set; }
        
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty("minDelayTarget", this.MinDelayTarget);
            writer.WriteOptionalProperty("maxDelayTarget", this.MaxDelayTarget);
            writer.WriteOptionalProperty("numRetries", this.NumRetries);
            writer.WriteOptionalProperty("numNoDelayRetries", this.NumNoDelayRetries);
            writer.WriteOptionalProperty("numMinDelayRetries", this.NumMinDelayRetries);
            writer.WriteOptionalProperty("numMaxDelayRetries", this.NumMaxDelayRetries);
            writer.WriteOptionalProperty("backoffFunction", this.BackoffFunction.GetDisplayName());
            writer.WriteOptionalProperty("maxReceivesPerSecond", this.MaxReceivesPerSecond);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public enum BackoffFunction
    {
        [Display("arithmetic")] Arithmetic,
        [Display("exponential")] Exponential,
        [Display("geometric")] Geometric,
        [Display("linear")] Linear,
    }
}