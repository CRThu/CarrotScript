using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript.Impl
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
                string s = ss[i];
                if (!isPrecise)
                {
                    if (s.StartsWith(c))
                    //if (s[0] == c)
                    {
                        return s;
                    }
                }
                else
                {
                    if (s.Equals(c.ToString()))
                    {
                        return s;
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
                string s = ss[i];
                if (!isPrecise)
                {
                    if (s.StartsWith(c))
                    //if (s[0..(c.Length)].Equals(c))
                    {
                        return s;
                    }
                }
                else
                {
                    if (s.Equals(c))
                    {
                        return s;
                    }
                }
            }
            return null;
        }
    }
}
