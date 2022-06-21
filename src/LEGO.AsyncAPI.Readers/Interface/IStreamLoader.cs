using System;
using System.IO;
using System.Threading.Tasks;

namespace LEGO.AsyncAPI.Readers.Interface
{
    public interface IStreamLoader
    {
        Task<Stream> LoadAsync(Uri uri);
        Stream Load(Uri uri);
    }
}