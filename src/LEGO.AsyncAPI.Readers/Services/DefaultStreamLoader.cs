// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Services
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.Interface;

    internal class DefaultStreamLoader : IStreamLoader
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public Stream Load(Uri uri)
        {
            try
            {
                switch (uri.Scheme)
                {
                    case "file":
                        return File.OpenRead(uri.AbsolutePath);
                    case "http":
                    case "https":
                        return HttpClient.GetStreamAsync(uri).GetAwaiter().GetResult();
                    default:
                        throw new ArgumentException("Unsupported scheme");
                }
            }
            catch (Exception ex)
            {

                throw new AsyncApiReaderException($"Something went wrong trying to fetch '{uri.OriginalString}. {ex.Message}'", ex);
            }
        }

        public async Task<Stream> LoadAsync(Uri uri)
        {
            try
            {
                switch (uri.Scheme)
                {
                    case "file":
                        return File.OpenRead(uri.AbsolutePath);
                    case "http":
                    case "https":
                        return await HttpClient.GetStreamAsync(uri);
                    default:
                        throw new ArgumentException("Unsupported scheme");
                }
            }
            catch (Exception ex)
            {

                throw new AsyncApiReaderException($"Something went wrong trying to fetch '{uri.OriginalString}'. {ex.Message}", ex);
            }
        }
    }
}