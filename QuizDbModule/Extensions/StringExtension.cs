using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuizDb.Module.Extensions
{
    public static class StringExtension
    {
        public static string RemoveParagraphTitle(this string str) 
        {
            var parts = str.Split(':');

            return !parts.Any() || parts[0].Contains("http") ? str : string.Join(":", parts, 1, parts.Length - 1);
        }

        public static string TrimWhitespace(this string str) => str.Replace("\n", string.Empty).Replace("&nbsp;", string.Empty).Trim();
    }
}
