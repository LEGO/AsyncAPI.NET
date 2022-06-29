// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// Path expression, the name in path is case-sensitive.
    /// </summary>
    public sealed class PathExpression : SourceExpression
    {
        /// <summary>
        /// path. string
        /// </summary>
        public const string Path = "path.";

        /// <summary>
        /// Initializes a new instance of the <see cref="PathExpression"/> class.
        /// </summary>
        /// <param name="name">The name string, it's case-insensitive.</param>
        public PathExpression(string name)
            : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw Error.ArgumentNullOrWhiteSpace(nameof(name));
            }
        }

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression
        {
            get
            {
                return Path + this.Value;
            }
        }

        /// <summary>
        /// Gets the name string.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Value;
            }
        }
    }
}