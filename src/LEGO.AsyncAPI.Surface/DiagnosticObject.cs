namespace LEGO.AsyncAPI.Surface;

using System;
using Json.Schema;
using Stubs;

public class DiagnosticObject
{
    public static DiagnosticObject OnParseError(Exception e) => new() { Error = e };
    public static DiagnosticObject OnValidateError(ValidatorResult validatorResult) => new() { ValidatorResult = validatorResult };
    public static DiagnosticObject OnReadError(Exception e) => new() { Error = e };

    public Exception Error { get; set; }
    public ValidatorResult ValidatorResult { get; set; }
}