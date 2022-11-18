namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Models.Bindings;

    /// <summary>
    /// Describes a protocol-specific binding.
    /// </summary>
    public interface IBinding : IAsyncApiReferenceable, IAsyncApiExtensible
    {
        public BindingType Type { get; }

        public string BindingVersion { get; set; }
    }
}
