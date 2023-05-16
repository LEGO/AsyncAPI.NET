namespace LEGO.AsyncAPI.Bindings.Sqs
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class Identifier : IAsyncApiElement
    {
        public string Arn { get; set; }

        public string Name { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty("arn", this.Arn);
            writer.WriteOptionalProperty("name", this.Name);
            writer.WriteEndObject();
        }
    }
}