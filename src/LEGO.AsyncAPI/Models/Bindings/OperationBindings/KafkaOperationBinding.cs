namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
{
    using LEGO.AsyncAPI.Any;
    using Newtonsoft.Json;

    public class KafkaOperationBinding : IOperationBinding
    {
        public Schema GroupId { get; set; }

        public Schema ClientId { get; set; }

        public string BindingVersion { get; set; }

        public IDictionary<string, IAny> Extensions { get; set; }
    }
}