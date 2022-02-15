namespace LEGO.AsyncAPI.Any
{
    public abstract class Primitive : IAny
    {
        public AnyType AnyType => AnyType.Primitive;

        public abstract PrimitiveType PrimitiveType { get; }
    }
}