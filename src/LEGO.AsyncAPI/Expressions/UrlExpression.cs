// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// Url expression.
    /// </summary>
    public sealed class UrlExpression : RuntimeExpression
    {
        /// <summary>
        /// $url string.
        /// </summary>
        public const string Url = "$url";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression { get; } = Url;

        /// <summary>
        /// Private constructor.
        /// </summary>
        public UrlExpression()
        {
        }
    }
}