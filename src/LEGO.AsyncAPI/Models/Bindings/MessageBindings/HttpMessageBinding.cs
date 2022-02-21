namespace LEGO.AsyncAPI.Models.Bindings.MessageBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class HttpMessageBinding : IMessageBinding
    {
        public Schema Headers { get; set; }

        public string BindingVersion { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}