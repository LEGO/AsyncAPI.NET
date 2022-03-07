// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class JsonStreamReader<T> : IStreamReader<T>
    {
        public T Read(Stream stream)
        {
            var jsonString = new StreamReader(stream).ReadToEnd();
            var root = JsonConvert.DeserializeObject<JToken>(jsonString, JsonSerializerUtils.Settings);
            root.ResolveReferences();

            return JsonConvert.DeserializeObject<T>(root.ToString(), JsonSerializerUtils.Settings);
        }
    }
}