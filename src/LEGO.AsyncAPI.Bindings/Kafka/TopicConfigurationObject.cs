// Copyright (c) The LEGO Group. All rights reserved.

using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings.Kafka
{

    public class TopicConfigurationObject : IAsyncApiExtensible
    {
        /// <summary>
        /// The cleanup.policy configuration option.
        /// </summary>
        public List<string> CleanupPolicy { get; set; }

        /// <summary>
        /// The retention.ms configuration option.
        /// </summary>
        public int? RetentionMilliseconds { get; set; }

        /// <summary>
        /// The retention.bytes configuration option.
        /// </summary>
        public int? RetentionBytes { get; set; }

        /// <summary>
        /// The delete.retention.ms configuration option.
        /// </summary>
        public int? DeleteRetentionMilliseconds { get; set; }

        /// <summary>
        /// The max.message.bytes configuration option.
        /// </summary>
        public int? MaxMessageBytes { get; set; }
        
        /// <summary>
        /// Extensions can be used to handle the configurations that are not covered by AsyncAPI bindings.
        /// </summary>
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalCollection(AsyncApiConstants.CleanupPolicy, this.CleanupPolicy, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty<int>(AsyncApiConstants.RetentionMilliseconds, this.RetentionMilliseconds);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.RetentionBytes, this.RetentionBytes);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.DeleteRetentionMilliseconds, this.DeleteRetentionMilliseconds);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.MaxMessageBytes, this.MaxMessageBytes);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}