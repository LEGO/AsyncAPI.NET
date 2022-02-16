namespace LEGO.AsyncAPI.Readers;

using System;

public class AsyncApiDiagnostic
{
    public static AsyncApiDiagnostic OnError(Exception e) => new() { Error = e };

    public Exception Error { get; set; }
}