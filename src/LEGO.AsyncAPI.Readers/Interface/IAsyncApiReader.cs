// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Interface
{
    using LEGO.AsyncAPI.Models;

    public interface IAsyncApiReader<TInput, TDiagnostic> where TDiagnostic : IDiagnostic
    {
        AsyncApiDocument Read(TInput input, out TDiagnostic diagnostic);
    }
}