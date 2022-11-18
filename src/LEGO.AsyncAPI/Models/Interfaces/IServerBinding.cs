// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Bindings;

    /// <summary>
    /// Describes a server-specific binding.
    /// </summary>
    public interface IServerBinding : IBinding, IAsyncApiExtensible
    {
    }
}
