namespace LEGO.AsyncAPI.Readers;

using System;

/// <summary>
/// Object containing errors that happened during AsyncApi parsing.
/// </summary>
public class AsyncApiDiagnostic
{
    public static AsyncApiDiagnostic OnError(Exception e) => new() { Error = e };

    public Exception Error { get; set; }
}