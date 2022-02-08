using Newtonsoft.Json;

namespace LEGO.AsyncAPI
{
    public class AsyncApiWriter<T>
    {
        public string Produce(T asyncApiDocument)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
            };
            return JsonConvert.SerializeObject(asyncApiDocument, jsonSerializerSettings);
        }
    }
}