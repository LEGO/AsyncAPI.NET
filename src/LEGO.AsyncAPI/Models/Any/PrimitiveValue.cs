namespace LEGO.AsyncAPI.Any;

public abstract class PrimitiveValue<T> : Primitive
{
    public abstract T? Value { get; set; }
}