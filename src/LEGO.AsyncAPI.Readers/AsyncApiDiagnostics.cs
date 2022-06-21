using System.Collections.Generic;
using LEGO.AsyncAPI.Models.Exceptions;
using LEGO.AsyncApi.Readers.Interface;

namespace LEGO.AsyncApi.Readers
{
    public class AsyncApiDiagnostic : IDiagnostic
    {
        public IList<AsyncApiError> Errors { get; set; } = new List<AsyncApiError>();
        
        public IList<AsyncApiError> Warnings { get; set; } = new List<AsyncApiError>();
        
        public AsyncApiSpecVersion SpecificationVersion { get; set; }
    }
}