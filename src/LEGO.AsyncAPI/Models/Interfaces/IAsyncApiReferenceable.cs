// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Writers;

    public interface IAsyncApiReferenceable : IAsyncApiSerializable
    {
        /// <summary>
        /// Reference object.
        /// </summary>
        AsyncApiReference Reference { get; set; }
    }
}