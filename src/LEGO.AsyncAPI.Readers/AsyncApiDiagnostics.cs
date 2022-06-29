// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.Interface;

    public class AsyncApiDiagnostic : IDiagnostic
    {
        public IList<AsyncApiError> Errors { get; set; } = new List<AsyncApiError>();

        public IList<AsyncApiError> Warnings { get; set; } = new List<AsyncApiError>();

        public AsyncApiVersion SpecificationVersion { get; set; }
    }
}
