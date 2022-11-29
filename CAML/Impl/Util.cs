using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAML.Impl
{
    public static class Util
    {
        /// <summary>
        /// 查找字符在字符串数组头部匹配, 若有则返回字符串, 若无则返回null
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="c"></param>
        /// <param name="isPrecise"></param>
        /// <returns></returns>
        public static string? FindMatchHeader(this string[] ss, char c, bool isPrecise = false)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                string op = Info.OPERATORS[i];
                if (!isPrecise)
                {
                    if (op[0] == c)
                    {
                        return op;
                    }
                }
                else
                {
                    if (op.Equals(c.ToString()))
                    {
                        return op;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查找字符串在字符串数组头部匹配, 若有则返回字符串, 若无则返回null
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="c"></param>
        /// <param name="isPrecise"></param>
        /// <returns></returns>
        public static string? FindMatchHeader(this string[] ss, string c, bool isPrecise = false)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                string op = Info.OPERATORS[i];
                if (!isPrecise)
                {
                    if (op[0..(c.Length)].Equals(c))
                    {
                        return op;
                    }
                }
                else
                {
                    if (op.Equals(c))
                    {
                        return op;
                    }
                }
            }
            return null;
        }
    }
}
