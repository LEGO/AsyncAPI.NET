namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Expressions;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The wrapper either for <see cref="IAsyncApiAny"/> or <see cref="RuntimeExpression"/>
    /// </summary>
    public class RuntimeExpressionAnyWrapper : IAsyncApiElement
    {
        private IAsyncApiAny any;
        private RuntimeExpression expression;

        /// <summary>
        /// Gets/Sets the <see cref="IAsyncApiAny"/>
        /// </summary>
        public IAsyncApiAny Any
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
        /// Gets/Set the <see cref="RuntimeExpression"/>
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
        /// Write <see cref="RuntimeExpressionAnyWrapper"/>
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