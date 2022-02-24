// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    public interface IInternalAsyncApiReader<T>
    {
        public T Read(Stream jsonDocument);
    }
}