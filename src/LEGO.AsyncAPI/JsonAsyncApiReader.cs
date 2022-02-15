using Newtonsoft.Json;

namespace LEGO.AsyncAPI
{
    public class JsonAsyncApiReader<T> : IAsyncApiReader<T>
    {
        public T Read(Stream stream)
        {
            return JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd(), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new ExtensionDataContractResolver(),
            });
        }
    }
}