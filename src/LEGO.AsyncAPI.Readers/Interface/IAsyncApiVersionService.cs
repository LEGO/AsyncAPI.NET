using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncApi.Readers.ParseNodes;

namespace LEGO.AsyncApi.Readers.Interface
{
    internal interface IAsyncApiVersionService
    {
        AsyncApiReference ConvertToAsyncApiReference(string reference, ReferenceType? type);
        
        T LoadElement<T>(ParseNode node) where T : IAsyncApiElement;
        
        AsyncApiDocument LoadDocument(RootNode rootNode);
    }
}