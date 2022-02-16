namespace LEGO.AsyncAPI.Models.Bindings.ServerBindings
{
    using LEGO.AsyncAPI.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class KafkaServerBinding : IServerBinding
    {
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}