﻿// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Models.Interfaces
{
    /// <summary>
    /// Describes a protocol-specific binding.
    /// </summary>
    public interface IBinding : IAsyncApiSerializable, IAsyncApiExtensible
    {
        public string BindingKey { get; }

        public string BindingVersion { get; set; }
    }
}
