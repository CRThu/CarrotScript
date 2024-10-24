using CarrotScript.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.Lang.Def;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarrotScript.Lexar
{
    public class CarrotXmlScanner
    {
        public static bool TryScan(CodeReader cr, out TokenType tokenType, out int length)
        {
            tokenType = TokenType.UNKNOWN;
            length = 0;
            while (cr.HasNext(length))
            {
                char c1 = cr.GetNextChar(length);
                char c2 = cr.GetNextChar(length + 1);

                if(c1=='<' && c2 == '?')
                {
                    tokenType = TokenType.XML_PI_ATTR_NAME;
                }
                else if(c1 == '<')
            }
        }
    }
}