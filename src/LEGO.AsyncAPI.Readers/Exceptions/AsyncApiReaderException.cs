using System;
using LEGO.AsyncAPI.Exceptions;
using SharpYaml.Serialization;

namespace LEGO.AsyncApi.Readers.Exceptions
{
    [Serializable]
    public class AsyncApiReaderException : AsyncApiException
    {
        public AsyncApiReaderException() { }
        
        public AsyncApiReaderException(string message) : base(message) { }
        
        public AsyncApiReaderException(string message, ParsingContext context) : base(message) {
            Pointer = context.GetLocation();
        }
        
        public AsyncApiReaderException(string message, YamlNode node) : base(message)
        {
            // This only includes line because using a char range causes tests to break due to CR/LF & LF differences
            // See https://tools.ietf.org/html/rfc5147 for syntax
            Pointer = $"#line={node.Start.Line}";
        }
        
        public AsyncApiReaderException(string message, Exception innerException) : base(message, innerException) { }

    }
}
