namespace LEGO.AsyncAPI.Models.Any
{
    public interface Primitive : IAny
    {
        public AnyType AnyType => AnyType.Primitive;

        public PrimitiveType PrimitiveType { get; }
    }
}