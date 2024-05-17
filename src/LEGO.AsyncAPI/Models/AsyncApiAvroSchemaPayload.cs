// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaPayload : IAsyncApiMessagePayload
    {
        public void SerializeV2(IAsyncApiWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
