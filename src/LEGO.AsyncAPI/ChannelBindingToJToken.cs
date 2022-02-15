using LEGO.AsyncAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI
{
    public class ChannelBindingToJToken
    {
        public static JToken Map(IDictionary<string, IChannelBinding> o)
        {
            var jsonSerializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new ExtensionDataContractResolver(),
            };
            return o == null ? null : JToken.FromObject(o, jsonSerializer);
        }
    }
}