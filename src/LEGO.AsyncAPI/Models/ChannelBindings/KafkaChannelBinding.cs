using LEGO.AsyncAPI.Any;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models.ChannelBindings;

public class KafkaChannelBinding : IChannelBinding
{
    public IDictionary<string, IAny> Extensions { get; set; }
}