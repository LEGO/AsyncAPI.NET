// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// Query expression, the name in query is case-sensitive.
    /// </summary>
    public sealed class QueryExpression : SourceExpression
    {
        /// <summary>
        /// query. string.
        /// </summary>
        public const string Query = "query.";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="name">The name string, it's case-insensitive.</param>
        public QueryExpression(string name)
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
                return Query + this.Value;
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