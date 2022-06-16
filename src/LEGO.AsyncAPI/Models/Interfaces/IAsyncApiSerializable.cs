// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Writers;

    public interface IAsyncApiSerializable
    {
        void SerializeV2(IAsyncApiWriter writer);
    }
}
