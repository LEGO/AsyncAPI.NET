namespace LEGO.AsyncAPI.Any
{
    public interface Primitive : IAny
    {
        public AnyType AnyType => AnyType.Primitive;

        public PrimitiveType PrimitiveType { get; }
    }
}