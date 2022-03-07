// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    public class JsonStringWriter<T> : IStringWriter<T>
    {
        public string Write(T @object)
        {
            return JsonConvert.SerializeObject(@object, JsonSerializerUtils.Settings);
        }
    }
}