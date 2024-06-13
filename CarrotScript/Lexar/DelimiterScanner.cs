using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;
using static CarrotScript.Lexar.DelimiterScanner.FSM_STATUS;

namespace CarrotScript.Lexar
{
    public static class DelimiterScanner
    {
        public enum FSM_STATUS
        {
            BEGIN,
            DELIMIT_SYM,
            DELIMIT_SPACE,
            END,
        }

        /// <summary>
        /// 扫描分隔符
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="tokenType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool TryScan(CodeReader cr, out TokenType tokenType, out int length)
        {
            tokenType = TokenType.UNKNOWN;
            FSM_STATUS state = 0;
            length = 0;
            while (cr.HasNext(length))
            {
                char c = cr.GetNextChar(length);
                switch (state)
                {
                    case BEGIN:
                        state = DELIMIT_SYM;
                        break;
                    case DELIMIT_SYM:
                        if (DELIMITERS_SYM.Contains(c))
                        {
                            length += 1;
                            if (!TokenDict.TryGetValue(c.ToString(), out tokenType))
                            {
                                TokenPosition start = cr.CurrentPosition;
                                throw new LexarNotSupportedException($"内部错误,未在{nameof(TokenDict)}解析到字符串:\"{c}\""
                                        , new TokenSpan(ref start, ref start));
                            }
                            state = END;
                        }
                        else
                        {
                            state = DELIMIT_SPACE;
                        }
                        break;
                    case DELIMIT_SPACE:
                        if (DELIMITERS_SPACE.Contains(c))
                        {
                            length += 1;
                            tokenType = TokenType.SPACE;
                            state = DELIMIT_SPACE;
                        }
                        else
                        {
                            state = END;
                        }
                        break;
                }
                if (state == END)
                    break;
            }

            return length != 0;
        }
    }
}
