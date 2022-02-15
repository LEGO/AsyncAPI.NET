using Newtonsoft.Json;

namespace LEGO.AsyncAPI
{
    public class JsonAsyncApiWriter<T> : IAsyncApiWriter<T>
    {
        public string Write(T asyncApiDocument)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new ExtensionDataContractResolver(),
            };
            return JsonConvert.SerializeObject(asyncApiDocument, jsonSerializerSettings);
        }
    }
}