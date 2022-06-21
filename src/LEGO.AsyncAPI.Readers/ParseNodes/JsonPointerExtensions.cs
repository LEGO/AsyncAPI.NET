using System;
using SharpYaml.Serialization;

namespace LEGO.AsyncApi.Readers.ParseNodes
{
    public static class JsonPointerExtensions
    {
        public static YamlNode Find(this JsonPointer currentPointer, YamlNode baseYamlNode)
        {
            if (currentPointer.Tokens.Length == 0)
            {
                return baseYamlNode;
            }

            try
            {
                var pointer = baseYamlNode;
                foreach (var token in currentPointer.Tokens)
                {
                    var sequence = pointer as YamlSequenceNode;

                    if (sequence != null)
                    {
                        pointer = sequence.Children[Convert.ToInt32(token)];
                    }
                    else
                    {
                        var map = pointer as YamlMappingNode;
                        if (map != null)
                        {
                            if (!map.Children.TryGetValue(new YamlScalarNode(token), out pointer))
                            {
                                return null;
                            }
                        }
                    }
                }

                return pointer;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}