namespace LEGO.AsyncAPI.Any;

public interface PrimitiveValue<T> : Primitive
{
    public T? Value { get; set; }
}