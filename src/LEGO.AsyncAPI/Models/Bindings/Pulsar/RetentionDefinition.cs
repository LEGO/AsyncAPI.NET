﻿namespace LEGO.AsyncAPI.Models.Bindings.Pulsar
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
            writer.WriteProperty(AsyncApiConstants.Time, this.Time);
            writer.WriteProperty(AsyncApiConstants.Size, this.Size);
            writer.WriteEndObject();
        }
    }
}
