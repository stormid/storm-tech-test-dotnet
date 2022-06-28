using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.TechTask.SharedKernel.Utilities
{
    public static class StringUtils
    {
        public static string ReplaceFirstOccurrence(this string source, string find, string replace)
        {
            int place = source.IndexOf(find);
            if (place > 0)
            {
                return source.Remove(place, find.Length).Insert(place, replace);
            }
            else
            {
                return source;
            }
        }

        public static string ReplaceLastOccurrence(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);
            if (place > 0)
            {
                return source.Remove(place, find.Length).Insert(place, replace);
            }
            else
            {
                return source;
            }
        }
    }
}
