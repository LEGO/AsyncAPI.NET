namespace LEGO.AsyncAPI.Any
{
    public abstract class Primitive<T> : IAny
    {
        public AnyType AnyType => AnyType.Primitive;

        public abstract PrimitiveType PrimitiveType { get; }
        
        public abstract T? Value { get; set; }
    }
}