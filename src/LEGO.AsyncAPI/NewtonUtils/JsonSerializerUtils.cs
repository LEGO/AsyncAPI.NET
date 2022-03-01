// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.NewtonUtils
{
    using LEGO.AsyncAPI.Resolvers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

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
                Converters = new List<JsonConverter> { new StringEnumConverter(true) },
            };
        }

        public static JsonSerializer GetSerializer()
        {
            return JsonSerializer.Create(GetSettings());
        }
    }
}