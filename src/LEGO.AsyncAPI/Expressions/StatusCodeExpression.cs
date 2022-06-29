// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// StatusCode expression.
    /// </summary>
    public sealed class StatusCodeExpression : RuntimeExpression
    {
        /// <summary>
        /// $statusCode string.
        /// </summary>
        public const string StatusCode = "$statusCode";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression { get; } = StatusCode;

        /// <summary>
        /// Private constructor.
        /// </summary>
        public StatusCodeExpression()
        {
        }
    }
}