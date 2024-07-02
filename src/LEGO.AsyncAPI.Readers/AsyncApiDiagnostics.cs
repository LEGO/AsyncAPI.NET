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

        public void Append(AsyncApiDiagnostic diagnosticToAdd, string fileNameToAdd = null)
        {
            var fileNameIsSupplied = !string.IsNullOrEmpty(fileNameToAdd);
            foreach (var error in diagnosticToAdd.Errors)
            {
                var errMsgWithFileName = fileNameIsSupplied ? $"[File: {fileNameToAdd}] {error.Message}" : error.Message;
                this.Errors.Add(new(error.Pointer, errMsgWithFileName));
            }

            foreach (var warning in diagnosticToAdd.Warnings)
            {
                var warnMsgWithFileName = fileNameIsSupplied ? $"[File: {fileNameToAdd}] {warning.Message}" : warning.Message;
                this.Warnings.Add(new(warning.Pointer, warnMsgWithFileName));
            }
        }
    }
}
