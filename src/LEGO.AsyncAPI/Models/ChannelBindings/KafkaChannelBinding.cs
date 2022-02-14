using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models.ChannelBindings;

public class KafkaChannelBinding : IChannelBinding
{
    public IDictionary<string, JToken> Extensions { get; set; }
}