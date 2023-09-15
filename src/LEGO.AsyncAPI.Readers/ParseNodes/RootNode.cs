// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System.Text.Json.Nodes;

    internal class RootNode : ParseNode
    {
        private readonly JsonNode jsonNode;

        public RootNode(
            ParsingContext context,
            JsonNode jsonNode)
            : base(context)
        {
            this.jsonNode = jsonNode;
        }

        public ParseNode Find(JsonPointer referencePointer)
        {
            if (referencePointer.Find(this.jsonNode) is not JsonNode jsonNode)
            {
                return null;
            }

            return Create(this.Context, jsonNode);
        }

        public MapNode GetMap()
        {
            return new MapNode(this.Context, this.jsonNode);
        }
    }
}