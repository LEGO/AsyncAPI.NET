// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Services
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using LEGO.AsyncAPI.Readers.Interface;

    internal class DefaultStreamLoader : IStreamLoader
    {
        private readonly Uri baseUrl;
        private HttpClient httpClient = new HttpClient();


        public DefaultStreamLoader(Uri baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public Stream Load(Uri uri)
        {
            var absoluteUri = new Uri(this.baseUrl, uri);
            switch (uri.Scheme)
            {
                case "file":
                    return File.OpenRead(absoluteUri.AbsolutePath);
                case "http":
                case "https":
                    return this.httpClient.GetStreamAsync(absoluteUri).GetAwaiter().GetResult();
                default:
                    throw new ArgumentException("Unsupported scheme");
            }
        }

        public async Task<Stream> LoadAsync(Uri uri)
        {
            var absoluteUri = new Uri(this.baseUrl, uri);

            switch (absoluteUri.Scheme)
            {
                case "file":
                    return File.OpenRead(absoluteUri.AbsolutePath);
                case "http":
                case "https":
                    return await this.httpClient.GetStreamAsync(absoluteUri);
                default:
                    throw new ArgumentException("Unsupported scheme");
            }
        }
    }
}