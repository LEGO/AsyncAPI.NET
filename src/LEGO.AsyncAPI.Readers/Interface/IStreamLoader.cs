// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Interface
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IStreamLoader
    {
        Task<Stream> LoadAsync(Uri uri);

        Stream Load(Uri uri);
    }
}