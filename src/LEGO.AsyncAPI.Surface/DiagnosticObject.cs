namespace LEGO.AsyncAPI.Surface
{
    using System;
    using Json.Schema;

    public class DiagnosticObject
    {
        public static DiagnosticObject OnParseError(Exception e) => new() { Error = e };
        public static DiagnosticObject OnValidateError(ValidationResults validationResults) => new() { ValidationResults = validationResults };
        public static DiagnosticObject OnReadError(Exception e) => new() { Error = e };

        public Exception Error { get; set; }
        public ValidationResults ValidationResults { get; set; }
    }
}