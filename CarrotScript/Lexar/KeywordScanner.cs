using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public static class KeywordScanner
    {
        /// <summary>
        /// 扫描关键词类型
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="tokenType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool TryScan(CodeReader cr, out TokenType tokenType, out int length)
        {
            tokenType = TokenType.UNKNOWN;
            length = 0;
            if (cr.HasNext(length))
            {
                var match = KEYWORDS_SYM.Where(kw => cr.GetNextSpan().StartsWith(kw)).ToArray();
                if (match.Length == 1)
                {
                    length = match[0].Length;
                    if (!TokenDict.TryGetValue(match[0], out tokenType))
                    {
                        TokenPosition start = cr.CurrentPosition;
                        throw new LexarNotSupportedException($"内部错误,未在{nameof(TokenDict)}解析到字符串:\"{match[0]}\""
                                , new TokenSpan(ref start, ref start));
                    }
                }
                else if (match.Length > 1)
                {
                    TokenPosition start = cr.CurrentPosition;
                    string matchList = string.Join('|', match);
                    throw new LexarNotSupportedException($"内部错误,在{nameof(TokenDict)}解析到多个匹配字符串:\"{matchList}\""
                            , new TokenSpan(ref start, ref start));
                }
            }
            return length != 0;
        }
    }
}
