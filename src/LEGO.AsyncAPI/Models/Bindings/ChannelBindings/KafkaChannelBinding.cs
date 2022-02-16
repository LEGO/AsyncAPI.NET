namespace LEGO.AsyncAPI.Models.Bindings.ChannelBindings
{
    using LEGO.AsyncAPI.Any;

    public class KafkaChannelBinding : IChannelBinding
    {
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}