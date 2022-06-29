// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    /// <summary>
    /// $response. expression.
    /// </summary>
    public sealed class ResponseExpression : RuntimeExpression
    {
        /// <summary>
        /// $response. string
        /// </summary>
        public const string Response = "$response.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseExpression"/> class.
        /// </summary>
        /// <param name="source">The source of the response.</param>
        public ResponseExpression(SourceExpression source)
        {
            this.Source = source ?? throw Error.ArgumentNull(nameof(source));
        }

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression => Response + this.Source.Expression;

        /// <summary>
        /// The <see cref="SourceExpression"/> expression.
        /// </summary>
        public SourceExpression Source { get; }
    }
}