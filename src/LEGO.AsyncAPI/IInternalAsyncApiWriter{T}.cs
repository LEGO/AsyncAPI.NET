// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    public interface IInternalAsyncApiWriter<T>
    {
        string Write(T asyncApiDocument);
    }
}