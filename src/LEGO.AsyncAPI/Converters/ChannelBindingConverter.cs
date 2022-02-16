using LEGO.AsyncAPI.Models.Bindings.ChannelBindings;

namespace LEGO.AsyncAPI.Converters
{
    using System.Collections.Immutable;
    using LEGO.AsyncAPI.Models;

    public class ChannelBindingConverter : BindingConverter<IChannelBinding>
    {
        protected override ImmutableDictionary<string, Type> GetBindingTypeMap()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Type>();
            builder.Add(new KeyValuePair<string, Type>("kafka", typeof(KafkaChannelBinding)));
            return builder.ToImmutable();
        }
    }
}