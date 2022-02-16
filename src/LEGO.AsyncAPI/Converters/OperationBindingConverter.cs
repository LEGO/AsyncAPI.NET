using LEGO.AsyncAPI.Models.Bindings.OperationBindings;

namespace LEGO.AsyncAPI.Converters
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;

    public class OperationBindingConverter : BindingConverter<IOperationBinding>
    {
        protected override ImmutableDictionary<string, Type> GetBindingTypeMap()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Type>();
            builder.Add(new KeyValuePair<string, Type>("kafka", typeof(KafkaOperationBinding)));
            builder.Add(new KeyValuePair<string, Type>("http", typeof(HttpOperationBinding)));
            return builder.ToImmutable();
        }
    }
}