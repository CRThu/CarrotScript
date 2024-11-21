using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript
{
    /// <summary>
    /// Token实现
    /// </summary>
    public class Token
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        /// <summary>
        /// Token类型
        /// </summary>
        public TokenType Type { get; set; }

        /// <summary>
        /// Token值
        /// </summary>
        public string Value { get; set; }

        //[JsonIgnore]
        /// <summary>
        /// 代码位置
        /// </summary>
        public TokenSpan Span { get; set; }

        ///// <summary>
        ///// 子Token
        ///// </summary>
        public Dictionary<string, string>? Attributes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public Token(TokenType tokenType, string value, TokenSpan span)
        {
            Type = tokenType;
            Value = value;
            Span = span;
            Attributes = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public Token(TokenType tokenType, string value, TokenSpan span, IDictionary<string, string>? attributes)
        {
            Type = tokenType;
            Value = value;
            Span = span;
            Attributes = (attributes != null && attributes.Count != 0) ? new Dictionary<string, string>(attributes) : null;
        }

        public override string ToString()
        {
            string tokenString =
                $"{{" +
                $"type = \"{Type}\"" +
                $", value = \"{RemoveUnprintableAscii(Value)}\"" +
                $", span = \"{Span}\"" +
                ((Attributes != null) ? ", " + AttributesToString() : "") +
                $"}}";
            //foreach (Token token in Children)
            //{
            //    tokenString += "\n\t";
            //    tokenString += token.ToString();
            //}
            return tokenString;
            /*
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            */
        }

        public string AttributesToString()
        {
            if (Attributes == null)
                return string.Empty;
            else
                return string.Join(", ", Attributes.Select(kv => $"{kv.Key} = \"{kv.Value}\""));
        }

        public string RemoveUnprintableAscii(string i)
        {
            return i.Replace(" ", "\\s").Replace("\t", "\\t").Replace("\r", "\\r").Replace("\n", "\\n");
        }
    }
}
