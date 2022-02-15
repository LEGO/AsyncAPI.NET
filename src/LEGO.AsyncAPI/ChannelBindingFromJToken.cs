using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.ChannelBindings;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI;

public class ChannelBindingFromJToken
{
    public static IDictionary<string, IChannelBinding> Map(JToken o)
    {
        return o?.ToObject<IDictionary<string, KafkaChannelBinding>>()
            .ToDictionary(k => k.Key, v => (IChannelBinding)v.Value);
    }
}