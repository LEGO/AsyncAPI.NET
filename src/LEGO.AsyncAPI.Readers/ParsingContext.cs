// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Readers.V2;

    public class ParsingContext
    {
        private readonly Stack<string> currentLocation = new ();

        internal Dictionary<string, Func<AsyncApiAny, IAsyncApiExtension>> ExtensionParsers
        {
            get;
            set;
        }

        = new ();

        internal Dictionary<string, IBindingParser<IServerBinding>> ServerBindingParsers { get; set; } = new ();

        internal Dictionary<string, IBindingParser<IChannelBinding>> ChannelBindingParsers { get; set; }
        
        internal Dictionary<string, IBindingParser<IOperationBinding>> OperationBindingParsers { get; set; } = new ();
        
        internal Dictionary<string, IBindingParser<IMessageBinding>> MessageBindingParsers { get; set; } = new ();

        internal RootNode RootNode { get; set; }

        internal List<AsyncApiTag> Tags { get; private set; } = new ();

        public AsyncApiDiagnostic Diagnostic { get; }

        public ParsingContext(AsyncApiDiagnostic diagnostic)
        {
            this.Diagnostic = diagnostic;
        }

        internal AsyncApiDocument Parse(JsonNode jsonNode)
        {
            this.RootNode = new RootNode(this, jsonNode);

            var inputVersion = GetVersion(this.RootNode);

            AsyncApiDocument doc;

            switch (inputVersion)
            {
                case string version when version.StartsWith("2"):
                    this.VersionService = new AsyncApiV2VersionService(this.Diagnostic);
                    doc = this.VersionService.LoadDocument(this.RootNode);
                    this.Diagnostic.SpecificationVersion = AsyncApiVersion.AsyncApi2_0;
                    break;

                default:
                    throw new AsyncApiUnsupportedSpecVersionException(inputVersion);
            }

            return doc;
        }

        internal T ParseFragment<T>(JsonNode jsonNode, AsyncApiVersion version) where T : IAsyncApiElement
        {
            var node = ParseNode.Create(this, jsonNode);

            T element = default(T);

            switch (version)
            {
                case AsyncApiVersion.AsyncApi2_0:
                    this.VersionService = new AsyncApiV2VersionService(this.Diagnostic);
                    element = this.VersionService.LoadElement<T>(node);
                    break;
            }

            return element;
        }

        private static string GetVersion(RootNode rootNode)
        {
            var versionNode = rootNode.Find(new JsonPointer("/asyncapi"));
            return versionNode?.GetScalarValue().Replace("\"", string.Empty);
        }

        internal IAsyncApiVersionService VersionService { get; set; }

        public void EndObject()
        {
            this.currentLocation.Pop();
        }

        public string GetLocation()
        {
            return "#/" + string.Join("/",
                this.currentLocation.Reverse().Select(s => s.Replace("~", "~0").Replace("/", "~1")).ToArray());
        }

        public void StartObject(string objectName)
        {
            this.currentLocation.Push(objectName);
        }
    }
}