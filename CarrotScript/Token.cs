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
    public struct Token
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
        }

        public override string ToString()
        {
            return $"{{\"type\" = \"{Type}\", \"value\" = \"{Value}\", \"span\" = \"{Span}\"}}";
            /*
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            */
        }
    }
}
