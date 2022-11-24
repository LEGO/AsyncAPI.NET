// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.Pulsar
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class RetentionDefinition : IAsyncApiElement
    {
        /// <summary>
        /// Time given in Minutes. 0 = Disable message retention (by default).
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Size given in MegaBytes. 0 = Disable message retention (by default).
        /// </summary>
        public int Size { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteRequiredProperty(AsyncApiConstants.Time, this.Time);
            writer.WriteRequiredProperty(AsyncApiConstants.Size, this.Size);
            writer.WriteEndObject();
        }
    }
}
