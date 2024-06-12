using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static CarrotScript.LangDef;

namespace CarrotScript
{
    /// <summary>
    /// Token位置
    /// </summary>
    /// <param name="line">代码行</param>
    /// <param name="col">代码列</param>
    public readonly struct TokenPosition(int line, int col)
    {
        public int Line { get; init; } = line;
        public int Col { get; init; } = col;
    }

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

        [JsonIgnore]
        /// <summary>
        /// 代码位置
        /// </summary>
        public TokenPosition Pos { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenType"></param>
        /// <param name="value"></param>
        public Token(TokenType tokenType, string value, TokenPosition tokenPosition)
        {
            Type = tokenType;
            Value = value;
            Pos = tokenPosition;
        }

        //public override readonly string ToString()
        //{
        //    string readableValue = Value
        //        .Replace("\n", "<NEWLINE>")
        //        .Replace(" ", "<SPACE>");

        //    return $"{{ {Type}: {readableValue} }}";
        //}

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
    }
}
