namespace LEGO.AsyncAPI.Readers.Serializers
{
    using System;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    internal class PrimitiveTypeNodeResolver : INodeTypeResolver
    {
        public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (nodeEvent is Scalar scalar)
            {
                if (bool.TryParse(scalar.Value, out var bool_value))
                {
                    currentType = typeof(bool);
                    return true;
                }

                if (int.TryParse(scalar.Value, out var int_value))
                {
                    currentType = typeof(int);
                    return true;
                }

                if (long.TryParse(scalar.Value, out var long_value))
                {
                    currentType = typeof(long);
                    return true;
                }

                if (double.TryParse(scalar.Value, out var double_value))
                {
                    currentType = typeof(double);
                    return true;
                }
            }

            return false;
        }
    }
}
