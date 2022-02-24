// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    public class InternalJsonAsyncApiWriter<T> : IInternalAsyncApiWriter<T>
    {
        public string Write(T asyncApiDocument)
        {
            return JsonConvert.SerializeObject(asyncApiDocument, JsonSerializerUtils.GetSettings());
        }
    }
}