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
                return _any;
            }
            set
            {
                _expression = null;
                _any = value;
            }
        }

        /// <summary>
        /// Gets/Set the <see cref="RuntimeExpression"/>
        /// </summary>
        public RuntimeExpression Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _any = null;
                _expression = value;
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

            if (_any != null)
            {
                writer.WriteAny(_any);
            }
            else if (_expression != null)
            {
                writer.WriteValue(_expression.Expression);
            }
        }
    }
}