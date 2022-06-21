using System.Collections.Generic;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models.Exceptions;
using LEGO.AsyncAPI.Readers.Interface;

namespace LEGO.AsyncAPI.Readers
{
    public class AsyncApiDiagnostic : IDiagnostic
    {
        public IList<AsyncApiError> Errors { get; set; } = new List<AsyncApiError>();
        
        public IList<AsyncApiError> Warnings { get; set; } = new List<AsyncApiError>();
        
        public AsyncApiSpecVersion SpecificationVersion { get; set; }
    }
}