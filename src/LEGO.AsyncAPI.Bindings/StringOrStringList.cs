using System;
using System.Linq;
using LEGO.AsyncAPI.Models.Any;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Bindings
{
    public class StringOrStringList : IAsyncApiElement
    {
        public StringOrStringList(IAsyncApiAny value)
        {
            this.Value = value switch
            {
                AsyncApiArray array => IsValidStringList(array) ? array : throw new ArgumentException($"{nameof(StringOrStringList)} value should contain string items."),
                AsyncApiPrimitive<string> => value,
                _ => throw new ArgumentException($"{nameof(StringOrStringList)} should be a string value or a string list.")
            };
        }

        public IAsyncApiAny Value { get; }
        
        public static StringOrStringList Parse(ParseNode node)
        {
            switch (node)
            {
                case ValueNode:
                    return new StringOrStringList(new AsyncApiString(node.GetScalarValue()));
                case ListNode:
                {
                    var asyncApiArray = new AsyncApiArray();
                    asyncApiArray.AddRange(node.CreateSimpleList(s => new AsyncApiString(s.GetScalarValue())));

                    return new StringOrStringList(asyncApiArray);
                }
                
                default:
                    throw new ArgumentException($"An error occured while parsing a {nameof(StringOrStringList)} node. " +
                                                $"Node should contain a string value or a list of strings.");
            }
        }
        
        private static bool IsValidStringList(AsyncApiArray array)
        {
            return array.All(x => x is AsyncApiPrimitive<string>);
        }
    }
}