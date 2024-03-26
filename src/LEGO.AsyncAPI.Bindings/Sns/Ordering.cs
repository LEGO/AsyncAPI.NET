// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Attributes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Ordering : IAsyncApiExtensible
    {
        /// <summary>
        /// What type of SNS Topic is this?
        /// </summary>
        public OrderingType Type { get; set; }

        /// <summary>
        /// True to turn on de-duplication of messages for a channel.
        /// </summary>
        public bool ContentBasedDeduplication { get; set; }

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteRequiredProperty("type", this.Type.GetDisplayName());
            writer.WriteOptionalProperty("contentBasedDeduplication", this.ContentBasedDeduplication);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }

    public enum OrderingType
    {
        [Display("standard")]
        Standard,
        [Display("FIFO")]
        Fifo,
    }
}