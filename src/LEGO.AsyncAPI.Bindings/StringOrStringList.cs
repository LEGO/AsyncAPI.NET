using System;
using System.Collections.Generic;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Bindings
{
    public class StringOrStringList : IAsyncApiElement
    {
        public string StringValue { get; set; }

        public List<string> StringList { get; set; }

        public void Serialize(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.StringValue != null)
            {
                writer.WriteValue(this.StringValue);
            }
            else
            {
                writer.WriteStartArray();
                foreach (var v in this.StringList)
                {
                    writer.WriteValue(v);
                }

                writer.WriteEndArray();
            }
        }
        
        public static StringOrStringList Parse(ParseNode node)
        {
            var stringOrStringList = new StringOrStringList();
            if (node is ValueNode)
            {
                stringOrStringList.StringValue = node.GetScalarValue();
            }
            else
            {
                stringOrStringList.StringList = node.CreateSimpleList(s => s.GetScalarValue());
            }

            return stringOrStringList;
        }
    }
}