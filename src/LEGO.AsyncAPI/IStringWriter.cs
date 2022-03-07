// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    public interface IStringWriter<T>
    {
        string Write(T @object);
    }
}