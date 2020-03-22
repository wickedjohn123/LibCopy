using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitTests
{
    static class RegexExpander
    {
        /// <summary>
        /// Expands a pattern-string such as "Test(a|A)" to new[]{"Testa","TestA"}
        /// </summary>
        /// <param name="input">A pattern-string containing one or more cases of (string1|string2|...|stringN).</param>
        public static List<string> Expand(string input)
        {
            var expandedCases = new List<string>();
            var casesLeftToCompute = new Queue<string>();

            casesLeftToCompute.Enqueue(input);

            while (casesLeftToCompute.Any())
            {

                string currentCase = casesLeftToCompute.Dequeue();
                var match = Regex.Match(currentCase, @"\((.*?\|.*?)\)");
                if (match.Success)
                {
                    var group = match.Groups[1];
                    var expression = group.Value;
                    var beforeString = currentCase.Substring(0, group.Index - 1);
                    var afterString = currentCase.Substring(group.Index + expression.Length + 1);
                    foreach (var combination in expression.Split('|'))
                    {
                        string newExpression = beforeString + combination + afterString;
                        casesLeftToCompute.Enqueue(newExpression);
                    }
                }
                else
                {
                    expandedCases.Add(currentCase);
                }
            }

            return expandedCases;
        }
    }
}
