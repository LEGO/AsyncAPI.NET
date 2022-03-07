// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    public interface IStreamReader<T>
    {
        public T Read(Stream jsonDocument);
    }
}