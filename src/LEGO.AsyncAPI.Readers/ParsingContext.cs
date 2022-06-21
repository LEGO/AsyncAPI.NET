using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Any;
using LEGO.AsyncAPI.Interfaces;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.Exceptions;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Readers;
using SharpYaml.Serialization;

namespace LEGO.AsyncAPI.Readers
{
    public class ParsingContext
    {
        private readonly Stack<string> _currentLocation = new Stack<string>();
        private readonly Dictionary<string, object> _tempStorage = new Dictionary<string, object>();
        private readonly Dictionary<object, Dictionary<string, object>> _scopedTempStorage = new Dictionary<object, Dictionary<string, object>>();
        private readonly Dictionary<string, Stack<string>> _loopStacks = new Dictionary<string, Stack<string>>();
        internal Dictionary<string, Func<IAsyncApiAny, AsyncApiVersion, IAsyncApiExtension>> ExtensionParsers { get; set; } = new Dictionary<string, Func<IAsyncApiAny, AsyncApiVersion, IAsyncApiExtension>>();
        internal RootNode RootNode { get; set; }
        internal List<AsyncApiTag> Tags { get; private set; } = new List<AsyncApiTag>();
        internal Uri BaseUrl { get; set; }

        public AsyncApiDiagnostic Diagnostic { get; }

        public ParsingContext(AsyncApiDiagnostic diagnostic)
        {
            Diagnostic = diagnostic;
        }
        
        internal AsyncApiDocument Parse(YamlDocument yamlDocument)
        {
            RootNode = new RootNode(this, yamlDocument);

            var inputVersion = GetVersion(RootNode);

            AsyncApiDocument doc;

            switch (inputVersion)
            {
                case string version when version == "2.0":
                    VersionService = new AsyncApiV2VersionService(Diagnostic);
                    doc = VersionService.LoadDocument(RootNode);
                    this.Diagnostic.SpecificationVersion = AsyncApiVersion.AsyncApi2_0;
                    break;

                case string version when version.StartsWith("3.0"):
                    VersionService = new AsyncApiV3VersionService(Diagnostic);
                    doc = VersionService.LoadDocument(RootNode);
                    this.Diagnostic.SpecificationVersion = AsyncApiVersion.AsyncApi3_0;
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
                case AsyncApiVersion.AsyncApi2_0:
                    VersionService = new AsyncApiV2VersionService(Diagnostic);
                    element = this.VersionService.LoadElement<T>(node);
                    break;

                case AsyncApiVersion.AsyncApi3_0:
                    this.VersionService = new AsyncApiV3VersionService(Diagnostic);
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

            versionNode = rootNode.Find(new JsonPointer("/swagger"));

            return versionNode?.GetScalarValue();
        }

        internal IAsyncApiVersionService VersionService { get; set; }

        public void EndObject()
        {
            _currentLocation.Pop();
        }

        public string GetLocation()
        {
            return "#/" + string.Join("/", _currentLocation.Reverse().Select(s=> s.Replace("~","~0").Replace("/","~1")).ToArray());
        }

        public T GetFromTempStorage<T>(string key, object scope = null)
        {
            Dictionary<string, object> storage;

            if (scope == null)
            {
                storage = _tempStorage;
            }
            else if (!_scopedTempStorage.TryGetValue(scope, out storage))
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
                storage = _tempStorage;
            }
            else if (!_scopedTempStorage.TryGetValue(scope, out storage))
            {
                storage = _scopedTempStorage[scope] = new Dictionary<string, object>();
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
            _currentLocation.Push(objectName);
        }

        public bool PushLoop(string loopId, string key)
        {
            Stack<string> stack;
            if (!_loopStacks.TryGetValue(loopId, out stack))
            {
                stack = new Stack<string>();
                _loopStacks.Add(loopId, stack);
            }

            if (!stack.Contains(key))
            {
                stack.Push(key);
                return true;
            }
            else
            {
                return false;  // Loop detected
            }
        }

        internal void ClearLoop(string loopid)
        {
            _loopStacks[loopid].Clear();
        }

        public void PopLoop(string loopid)
        {
            if (_loopStacks[loopid].Count > 0)
            {
                _loopStacks[loopid].Pop();
            }
        }

    }
}
