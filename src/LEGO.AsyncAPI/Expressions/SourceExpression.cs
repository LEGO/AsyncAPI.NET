// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    using LEGO.AsyncAPI.Exceptions;

    /// <summary>
    /// Source expression.
    /// </summary>
    public abstract class SourceExpression : RuntimeExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryExpression"/> class.
        /// </summary>
        /// <param name="value">The value string.</param>
        protected SourceExpression(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        protected string Value { get; }

        /// <summary>
        /// Build the source expression from input string.
        /// </summary>
        /// <param name="expression">The source expression.</param>
        /// <returns>The built source expression.</returns>
        public new static SourceExpression Build(string expression)
        {
            if (!string.IsNullOrWhiteSpace(expression))
            {
                var expressions = expression.Split('.');
                if (expressions.Length == 2)
                {
                    if (expression.StartsWith(HeaderExpression.Header))
                    {
                        // header.
                        return new HeaderExpression(expressions[1]);
                    }

                    if (expression.StartsWith(QueryExpression.Query))
                    {
                        // query.
                        return new QueryExpression(expressions[1]);
                    }

                    if (expression.StartsWith(PathExpression.Path))
                    {
                        // path.
                        return new PathExpression(expressions[1]);
                    }
                }

                // body
                if (expression.StartsWith(BodyExpression.Body))
                {
                    var subString = expression.Substring(BodyExpression.Body.Length);
                    if (string.IsNullOrEmpty(subString))
                    {
                        return new BodyExpression();
                    }

                    return new BodyExpression(new JsonPointer(subString));
                }
            }

            throw new AsyncApiException(string.Format("The source expression '{0}' has invalid format.", expression));
        }
    }
}
