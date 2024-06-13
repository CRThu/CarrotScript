using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lexar.IdentifierScanner.FSM_STATUS;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public static class IdentifierScanner
    {
        public enum FSM_STATUS
        {
            BEGIN,
            FIRST_CHAR,
            STR,
            END,
        }

        /// <summary>
        /// 扫描标识符类型
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
                        state = FIRST_CHAR;
                        break;
                    case FIRST_CHAR:
                        if (IDENTIFIER_FIRST_CHAR.Contains(c))
                        {
                            length += 1;
                            state = STR;
                        }
                        else
                        {
                            state = END;
                        }
                        break;
                    case STR:
                        if (IDENTIFIER_CHARS.Contains(c))
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
                tokenType = TokenType.IDENTIFIER;
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
