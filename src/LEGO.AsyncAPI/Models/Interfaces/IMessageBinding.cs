namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Bindings;

    /// <summary>
    /// Describes a message-specific binding.
    /// </summary>
    public interface IMessageBinding : IBinding, IAsyncApiExtensible
    {
    }
}
