namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API object.
    /// </summary>
    public class Object : Dictionary<string, IAny>, IAny
    {
        /// <summary>
        /// Type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Object;
    }
}