using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarrotScript
{
    public static class Version
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        }
    }
}
