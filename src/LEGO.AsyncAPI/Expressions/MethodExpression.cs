// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// Method expression.
    /// </summary>
    public sealed class MethodExpression : RuntimeExpression
    {
        /// <summary>
        /// $method. string.
        /// </summary>
        public const string Method = "$method";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression { get; } = Method;
    }
}