using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lexar.NumericScanner.FSM_STATUS;
using static CarrotScript.Lang.Def;

namespace CarrotScript.Lexar
{
    public static class NumericScanner
    {
        public enum FSM_STATUS
        {
            BEGIN,
            INT,
            FRAC,
            EXPSIGN,
            EXP,
            END,
        }

        /// <summary>
        /// 扫描数字类型
        /// </summary>
        /// <param name="cr"></param>
        /// <param name="tokenType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool TryScan(CodeReader cr, out TokenType tokenType, out int length)
        {
            // STATE:   0 1    2 3 4
            // NUMBER:  {-}123.456E-05
            FSM_STATUS state = 0;
            length = 0;
            while (cr.HasNext(length))
            {
                char c = cr.GetNextChar(length);
                switch (state)
                {
                    case BEGIN:
                        state = INT;
                        break;
                    case INT:
                        // INT {123}{.}456E-05
                        if (NUMERIC_DIGITS.Contains(c))
                        {
                            length += 1;
                            state = INT;
                        }
                        else if (NUMERIC_DOTS.Contains(c))
                        {
                            length += 1;
                            state = FRAC;
                        }
                        else
                        {
                            state = EXP;
                        }
                        break;
                    case FRAC:
                        // FRAC 123.{456}{E}-05
                        if (NUMERIC_DIGITS.Contains(c))
                        {
                            length += 1;
                            state = FRAC;
                        }
                        else if (NUMERIC_EXPSYM.Contains(c))
                        {
                            state = EXPSIGN;
                        }
                        else
                        {
                            state = END;
                        }
                        break;
                    case EXPSIGN:
                        // EXPSIGN 123.456E{-}05
                        if (NUMERIC_SIGN.Contains(c))
                        {
                            length += 1;
                        }
                        state = EXP;
                        break;
                    case EXP:
                        // EXP 123.456E-{05}
                        if (NUMERIC_DIGITS.Contains(c))
                        {
                            length += 1;
                            state = EXP;
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
                tokenType = TokenType.NUMERIC;
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
