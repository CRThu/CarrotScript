using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CarrotScript.LangDef;

namespace CarrotScript
{
    public static class Match
    {
        /// <summary>
        /// 查找字符串在字符串数组头部匹配, 若有则返回字符串, 若无则返回false
        /// </summary>
        /// <param name="matchArray"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool TryFindMatch(this string[] matchArray, ReadOnlySpan<char> s, out string? matchWord)
        {
            for (int i = 0; i < matchArray.Length; i++)
            {
                if (s.StartsWith(matchArray[i]))
                {
                    matchWord = matchArray[i];
                    return true;
                }
            }

            matchWord = null;
            return false;
        }
    }
}
