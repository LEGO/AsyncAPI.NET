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

        public void Append(AsyncApiDiagnostic diagnosticToAdd)
        {
            foreach (var error in diagnosticToAdd.Errors)
            {
                this.Errors.Add(new(error.Pointer, error.Message));
            }

            foreach (var warning in diagnosticToAdd.Warnings)
            {
                this.Warnings.Add(new(warning.Pointer, warning.Message));
            }
        }
    }
}
