namespace LEGO.AsyncAPI.Models.Any
{
    public interface PrimitiveValue<T> : Primitive
    {
        public T? Value { get; set; }
    }
}