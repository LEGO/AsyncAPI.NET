// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Expressions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The wrapper either for <see cref="AsyncApiAny"/> or <see cref="RuntimeExpression"/>.
    /// </summary>
    public class RuntimeExpressionAnyWrapper : IAsyncApiElement
    {
        private AsyncApiAny any;
        private RuntimeExpression expression;

        /// <summary>
        /// Gets/Sets the <see cref="AsyncApiAny"/>.
        /// </summary>
        public AsyncApiAny Any
        {
            get
            {
                return this.any;
            }

            set
            {
                this.expression = null;
                this.any = value;
            }
        }

        /// <summary>
        /// Gets/Set the <see cref="RuntimeExpression"/>.
        /// </summary>
        public RuntimeExpression Expression
        {
            get
            {
                return this.expression;
            }

            set
            {
                this.any = null;
                this.expression = value;
            }
        }

        /// <summary>
        /// Write <see cref="RuntimeExpressionAnyWrapper"/>.
        /// </summary>
        public void WriteValue(IAsyncApiWriter writer)
        {
            if (writer == null)
            {
                throw Error.ArgumentNull(nameof(writer));
            }

            if (this.any != null)
            {
                writer.WriteAny(this.any);
            }
            else if (this.expression != null)
            {
                writer.WriteValue(this.expression.Expression);
            }
        }
    }
}