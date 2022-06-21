using LEGO.AsyncAPI.Expressions;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// The wrapper either for <see cref="IAsyncApiAny"/> or <see cref="RuntimeExpression"/>
    /// </summary>
    public class RuntimeExpressionAnyWrapper : IAsyncApiElement
    {
        private IAsyncApiAny _any;
        private RuntimeExpression _expression;

        /// <summary>
        /// Gets/Sets the <see cref="IAsyncApiAny"/>
        /// </summary>
        public IAsyncApiAny Any
        {
            get
            {
                return this._any;
            }
            set
            {
                this._expression = null;
                this._any = value;
            }
        }

        /// <summary>
        /// Gets/Set the <see cref="RuntimeExpression"/>
        /// </summary>
        public RuntimeExpression Expression
        {
            get
            {
                return this._expression;
            }
            set
            {
                this._any = null;
                this._expression = value;
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

            if (this._any != null)
            {
                writer.WriteAny(this._any);
            }
            else if (this._expression != null)
            {
                writer.WriteValue(this._expression.Expression);
            }
        }
    }
}