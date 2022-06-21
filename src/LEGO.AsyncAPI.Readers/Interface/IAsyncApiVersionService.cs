using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers.Interface
{
    internal interface IAsyncApiVersionService
    {
        AsyncApiReference ConvertToAsyncApiReference(string reference, ReferenceType? type);
        
        T LoadElement<T>(ParseNode node) where T : IAsyncApiElement;
        
        AsyncApiDocument LoadDocument(RootNode rootNode);
    }
}