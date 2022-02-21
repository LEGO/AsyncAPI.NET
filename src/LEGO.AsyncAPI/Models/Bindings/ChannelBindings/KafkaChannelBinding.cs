namespace LEGO.AsyncAPI.Models.Bindings.ChannelBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class KafkaChannelBinding : IChannelBinding
    {
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}