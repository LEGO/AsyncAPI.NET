namespace LEGO.AsyncAPI.Surface.Stubs;

using Json.Schema;

public class ValidatorResult
{
    public bool IsValid { get; init; }
    public ValidationResults ValidationResults { get; init; }
}