// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Expressions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// String literal with embedded expressions
    /// </summary>
    public class CompositeExpression : RuntimeExpression
    {
        private readonly string template;
        private Regex expressionPattern = new Regex(@"{(?<exp>\$[^}]*)");

        /// <summary>
        /// Expressions embedded into string literal
        /// </summary>
        public List<RuntimeExpression> ContainedExpressions = new List<RuntimeExpression>();

        /// <summary>
        /// Create a composite expression from a string literal with an embedded expression
        /// </summary>
        /// <param name="expression"></param>
        public CompositeExpression(string expression)
        {
            this.template = expression;

            // Extract subexpressions and convert to RuntimeExpressions
            var matches = this.expressionPattern.Matches(expression);

            foreach (var item in matches.Cast<Match>())
            {
                var value = item.Groups["exp"].Captures.Cast<Capture>().First().Value;
                this.ContainedExpressions.Add(RuntimeExpression.Build(value));
            }
        }

        /// <summary>
        /// Return original string literal with embedded expression
        /// </summary>
        public override string Expression => this.template;
    }
}