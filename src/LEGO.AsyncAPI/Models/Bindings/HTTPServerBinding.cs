using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace LEGO.AsyncAPI.Models.Bindings;

public class HttpServerBinding: IServerBinding
{
    public string Type{ get; set; }

    public string Method{ get; set; }

    public Schema Query{ get; set; }

    public string BindingVersion{ get; set; }

    /// <inheritdoc/>
    [JsonExtensionData]
    public IDictionary<string, JToken> Extensions { get; set; }
}