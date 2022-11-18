namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Bindings;

    /// <summary>
    /// Describes an operation-specific binding.
    /// </summary>
    public interface IOperationBinding : IBinding, IAsyncApiExtensible
    {
    }
}
