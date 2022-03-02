// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class InternalJsonAsyncApiReader<T> : IInternalAsyncApiReader<T>
    {
        public T Read(Stream stream)
        {
            var jsonString = new StreamReader(stream).ReadToEnd();
            var root = JsonConvert.DeserializeObject<JToken>(jsonString, JsonSerializerUtils.GetSettings());
            root.ResolveReferences();

            return JsonConvert.DeserializeObject<T>(root.ToString(), JsonSerializerUtils.GetSettings());
        }
    }
}