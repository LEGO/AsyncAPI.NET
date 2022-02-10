namespace LEGO.AsyncAPI
{
    using Newtonsoft.Json;

    public class AsyncApiWriterNewtonJson<T> : IAsyncApiWriter<T>
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