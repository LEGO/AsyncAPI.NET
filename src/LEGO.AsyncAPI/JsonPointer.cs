// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System;
    using System.Linq;

    /// <summary>
    /// JSON pointer.
    /// </summary>
    public class JsonPointer
    {
        /// <summary>
        /// Initializes the <see cref="JsonPointer"/> class.
        /// </summary>
        /// <param name="pointer">Pointer as string.</param>
        public JsonPointer(string pointer)
        {
            this.Tokens = string.IsNullOrEmpty(pointer) || pointer == "/"
                ? new string[0]
                : pointer.Split('/').Skip(1).Select(this.Decode).ToArray();
        }

        /// <summary>
        /// Initializes the <see cref="JsonPointer"/> class.
        /// </summary>
        /// <param name="tokens">Pointer as tokenized string.</param>
        private JsonPointer(string[] tokens)
        {
            this.Tokens = tokens;
        }

        /// <summary>
        /// Tokens.
        /// </summary>
        public string[] Tokens { get; }

        /// <summary>
        /// Gets the parent pointer.
        /// </summary>
        public JsonPointer ParentPointer
        {
            get
            {
                if (this.Tokens.Length == 0)
                {
                    return null;
                }

                return new JsonPointer(this.Tokens.Take(this.Tokens.Length - 1).ToArray());
            }
        }

        /// <summary>
        /// Decode the string.
        /// </summary>
        private string Decode(string token)
        {
            return Uri.UnescapeDataString(token).Replace("~1", "/").Replace("~0", "~");
        }

        /// <summary>
        /// Gets the string representation of this JSON pointer.
        /// </summary>
        public override string ToString()
        {
            return "/" + string.Join("/", this.Tokens);
        }
    }
}