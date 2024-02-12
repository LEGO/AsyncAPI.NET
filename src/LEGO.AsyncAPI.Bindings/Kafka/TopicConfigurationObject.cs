// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Kafka
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

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
        
        /// <summary>
        /// The confluent.key.schema.validation configuration option.
        /// </summary>
        public bool? ConfluentKeySchemaValidation { get; set; }
        
        /// <summary>
        /// The confluent.key.subject.name.strategy configuration option.
        /// </summary>
        public string ConfluentKeySubjectName { get; set; }
        
        /// <summary>
        /// The confluent.value.schema.validation configuration option.
        /// </summary>
        public bool? ConfluentValueSchemaValidation { get; set; }
        
        /// <summary>
        /// The confluent.value.subject.name.strategy configuration option.
        /// </summary>
        public string ConfluentValueSubjectName { get; set; }

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
            writer.WriteOptionalProperty<bool>(AsyncApiConstants.ConfluentKeySchemaValidation, this.ConfluentKeySchemaValidation);
            writer.WriteOptionalProperty(AsyncApiConstants.ConfluentKeySubjectName, this.ConfluentKeySubjectName);
            writer.WriteOptionalProperty<bool>(AsyncApiConstants.ConfluentValueSchemaValidation, this.ConfluentValueSchemaValidation);
            writer.WriteOptionalProperty(AsyncApiConstants.ConfluentValueSubjectName, this.ConfluentValueSubjectName);
            writer.WriteEndObject();
        }
    }
}