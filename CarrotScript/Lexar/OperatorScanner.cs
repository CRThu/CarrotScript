using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public static class OperatorScanner
    {
        /// <summary>
        /// 扫描符号类型
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
                //匹配最长
                var match = OPERATORS_SYM.Where(kw => cr.GetNextSpan().StartsWith(kw)).ToArray();
                if (match.Length >= 1)
                {
                    var matchLongest = match.OrderByDescending(kw => kw.Length).First();
                    length = matchLongest.Length;
                    if (!TokenDict.TryGetValue(matchLongest, out tokenType))
                    {
                        TokenPosition start = cr.CurrentPosition;
                        throw new LexarNotSupportedException($"内部错误,未在{nameof(TokenDict)}解析到字符串:\"{matchLongest}\""
                                , new TokenSpan(ref start, ref start));
                    }
                }
            }
            return length != 0;
        }
    }
}
