// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Exceptions;
    using LEGO.AsyncAPI.Readers.Interface;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using SharpYaml.Serialization;

    public class ParsingContext
    {
        private readonly Stack<string> _currentLocation = new();
        private readonly Dictionary<string, object> _tempStorage = new();
        private readonly Dictionary<object, Dictionary<string, object>> _scopedTempStorage = new();
        private readonly Dictionary<string, Stack<string>> loopStacks = new();

        internal Dictionary<string, Func<IAsyncApiAny, AsyncApiVersion, IAsyncApiExtension>> ExtensionParsers
        {
            get;
            set;
        }

        = new();

        internal RootNode RootNode { get; set; }

        internal List<AsyncApiTag> Tags { get; private set; } = new();

        public AsyncApiDiagnostic Diagnostic { get; }

        public ParsingContext(AsyncApiDiagnostic diagnostic)
        {
            this.Diagnostic = diagnostic;
        }

        internal AsyncApiDocument Parse(YamlDocument yamlDocument)
        {
            this.RootNode = new RootNode(this, yamlDocument);

            var inputVersion = GetVersion(this.RootNode);

            AsyncApiDocument doc;

            switch (inputVersion)
            {
                case string version when version.StartsWith("2.3"):
                    this.VersionService = new AsyncApiVersionService(this.Diagnostic);
                    doc = this.VersionService.LoadDocument(this.RootNode);
                    this.Diagnostic.SpecificationVersion = AsyncApiVersion.AsyncApi2_3_0;
                    break;

                default:
                    throw new AsyncApiUnsupportedSpecVersionException(inputVersion);
            }

            return doc;
        }

        internal T ParseFragment<T>(YamlDocument yamlDocument, AsyncApiVersion version) where T : IAsyncApiElement
        {
            var node = ParseNode.Create(this, yamlDocument.RootNode);

            T element = default(T);

            switch (version)
            {
                case AsyncApiVersion.AsyncApi2_3_0:
                    this.VersionService = new AsyncApiVersionService(this.Diagnostic);
                    element = this.VersionService.LoadElement<T>(node);
                    break;
            }

            return element;
        }

        private static string GetVersion(RootNode rootNode)
        {
            var versionNode = rootNode.Find(new JsonPointer("/asyncapi"));

            if (versionNode != null)
            {
                return versionNode.GetScalarValue();
            }

            return versionNode?.GetScalarValue();
        }

        internal IAsyncApiVersionService VersionService { get; set; }

        public void EndObject()
        {
            this._currentLocation.Pop();
        }

        public string GetLocation()
        {
            return "#/" + string.Join("/",
                this._currentLocation.Reverse().Select(s => s.Replace("~", "~0").Replace("/", "~1")).ToArray());
        }

        public T GetFromTempStorage<T>(string key, object scope = null)
        {
            Dictionary<string, object> storage;

            if (scope == null)
            {
                storage = this._tempStorage;
            }
            else if (!this._scopedTempStorage.TryGetValue(scope, out storage))
            {
                return default(T);
            }

            return storage.TryGetValue(key, out var value) ? (T)value : default(T);
        }

        public void SetTempStorage(string key, object value, object scope = null)
        {
            Dictionary<string, object> storage;

            if (scope == null)
            {
                storage = this._tempStorage;
            }
            else if (!this._scopedTempStorage.TryGetValue(scope, out storage))
            {
                storage = this._scopedTempStorage[scope] = new Dictionary<string, object>();
            }

            if (value == null)
            {
                storage.Remove(key);
            }
            else
            {
                storage[key] = value;
            }
        }

        public void StartObject(string objectName)
        {
            this._currentLocation.Push(objectName);
        }

        public bool PushLoop(string loopId, string key)
        {
            Stack<string> stack;
            if (!this.loopStacks.TryGetValue(loopId, out stack))
            {
                stack = new Stack<string>();
                this.loopStacks.Add(loopId, stack);
            }

            if (!stack.Contains(key))
            {
                stack.Push(key);
                return true;
            }

            return false; // Loop detected
        }

        internal void ClearLoop(string loopid)
        {
            this.loopStacks[loopid].Clear();
        }

        public void PopLoop(string loopid)
        {
            if (this.loopStacks[loopid].Count > 0)
            {
                this.loopStacks[loopid].Pop();
            }
        }
    }
}