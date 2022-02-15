namespace LEGO.AsyncAPI.Surface;

using Models;

public class ReaderReadResult
{
    public AsyncApiDocument Document { get; set; }
    public DiagnosticObject DiagnosticObject { get; set; }
    public bool HasError => DiagnosticObject?.Error != null;
    public bool HasValidationError => DiagnosticObject?.ValidatorResult != null;
}