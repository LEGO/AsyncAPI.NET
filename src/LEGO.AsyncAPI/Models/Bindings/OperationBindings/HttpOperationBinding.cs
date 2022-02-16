namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
{
    using LEGO.AsyncAPI.Any;
    using Newtonsoft.Json;

    public class HttpOperationBinding : IOperationBinding
    {
        public string Type { get; set; }

        public string Method { get; set; }

        public Schema Query { get; set; }

        public string BindingVersion { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}