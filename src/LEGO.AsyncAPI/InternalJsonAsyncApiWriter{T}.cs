// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    public class InternalJsonAsyncApiWriter<T> : IInternalAsyncApiWriter<T>
    {
        public string Write(T asyncApiObject)
        {
            return JsonConvert.SerializeObject(asyncApiObject, JsonSerializerUtils.GetSettings());
        }
    }
}