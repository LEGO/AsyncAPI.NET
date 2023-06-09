namespace LEGO.AsyncAPI.Bindings.Sns
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;


    public class Identifier : IAsyncApiExtensible
    {
        public string Url { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Arn { get; set; }

        public string Name { get; set; }
        
        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteStartObject();
            writer.WriteOptionalProperty("url", this.Url);
            writer.WriteOptionalProperty("email", this.Email);
            writer.WriteOptionalProperty("phone", this.Phone);
            writer.WriteOptionalProperty("arn", this.Arn);
            writer.WriteOptionalProperty("name", this.Name);
            writer.WriteExtensions(this.Extensions);
            writer.WriteEndObject();
        }
    }
}