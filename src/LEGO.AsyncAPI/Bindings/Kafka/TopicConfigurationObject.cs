// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;
using System;
using System.Collections.Generic;

namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    public class TopicConfigurationObject : IAsyncApiElement
    {
        /// <summary>
        /// The cleanup.policy configuration option.
        /// </summary>
        public List<string> CleanupPolicy { get; set; }

        /// <summary>
        /// The retention.ms configuration option.
        /// </summary>
        public int? RetentionMiliseconds { get; set; }

        /// <summary>
        /// The retention.bytes configuration option.
        /// </summary>
        public int? RetentionBytes { get; set; }

        /// <summary>
        /// The delete.retention.ms configuration option.
        /// </summary>
        public int? DeleteRetentionMiliseconds { get; set; }

        /// <summary>
        /// The max.message.bytes configuration option.
        /// </summary>
        public int? MaxMessageBytes { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalCollection(AsyncApiConstants.CleanupPolicy, this.CleanupPolicy, (w, s) => w.WriteValue(s));
            writer.WriteOptionalProperty<int>(AsyncApiConstants.RetentionMiliseconds, this.RetentionMiliseconds);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.RetentionBytes, this.RetentionBytes);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.DeleteRetentionMiliseconds, this.DeleteRetentionMiliseconds);
            writer.WriteOptionalProperty<int>(AsyncApiConstants.MaxMessageBytes, this.MaxMessageBytes);
            writer.WriteEndObject();
        }
    }
}