// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Bindings.MessageBindings;

namespace LEGO.AsyncAPI.Models.Interfaces
{
    /// <summary>
    /// Describes a message-specific binding.
    /// </summary>
    public interface IMessageBinding : IBinding, IAsyncApiExtensible
    {
        public MessageBindingType Type { get; }
        public string BindingVersion { get; set; }
    }
}
