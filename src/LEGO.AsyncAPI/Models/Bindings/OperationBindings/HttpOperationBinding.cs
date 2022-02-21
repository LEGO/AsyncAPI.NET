namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class HttpOperationBinding : IOperationBinding
    {
        public string Type { get; set; }

        public string Method { get; set; }

        public Schema Query { get; set; }

        public string BindingVersion { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}