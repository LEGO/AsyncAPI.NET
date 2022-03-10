// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.NewtonUtils
{
    using LEGO.AsyncAPI.Resolvers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Static Utils class, exposes JSON settings and JSON serializer.
    /// </summary>
    public static class JsonSerializerUtils
    {
        private static readonly List<JsonConverter> JsonConverters = new() { new StringEnumConverter(true) };

        static JsonSerializerUtils()
        {
            Serializer = JsonSerializer.Create(Settings);
        }

        /// <summary>
        /// Serializer singleton with settings defined below.
        /// </summary>
        public static JsonSerializer Serializer { get; }

        /// <summary>
        /// Settings for JSON serializer with AsyncApiContractResolver as contract resolver.
        /// </summary>
        public static JsonSerializerSettings Settings = new ()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new AsyncApiContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = JsonConverters,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
            };
    }
}