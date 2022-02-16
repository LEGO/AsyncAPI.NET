namespace LEGO.AsyncAPI.NewtonUtils
{
    using Newtonsoft.Json;

    public static class JsonSerializerUtils
    {
        public static JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new AsyncApiContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.Include,
            };
        }

        public static JsonSerializer GetSerializer()
        {
            return JsonSerializer.Create(GetSettings());
        }
    }
}