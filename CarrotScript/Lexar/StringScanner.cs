using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lexar.StringScanner.FSM_STATUS;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public static class StringScanner
    {
        public enum FSM_STATUS
        {
            BEGIN,
            STR,
            END,
        }

        /// <summary>
        /// 扫描字符串类型
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="tokenType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool TryScan(CodeReader cr, out TokenType tokenType, out int length)
        {
            FSM_STATUS state = 0;
            length = 0;
            while (cr.HasNext(length))
            {
                char c = cr.GetNextChar(length);
                switch (state)
                {
                    case BEGIN:
                        state = STR;
                        break;
                    case STR:
                        if (STRINGS_CHARS.Contains(c))
                        {
                            length += 1;
                            state = STR;
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

            if (length != 0)
            {
                tokenType = TokenType.STRING;
                return true;
            }
            else
            {
                tokenType = TokenType.UNKNOWN;
                return false;
            }
        }
    }
}
