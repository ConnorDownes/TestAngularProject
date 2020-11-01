using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Injects a string every x spaces.
        /// Injecting " " into "Hello" with a position of 1 will result in "H e l l o".
        /// </summary>
        /// <param name="stringToInject">String which will be injected.</param>
        /// <param name="position">The string will be injected every x spaces defined by this position param. Must satisfy 1 <= position < input.Length.</param>
        /// <returns>The modified string or input string if an invalid position was passed in.</returns>
        public static string Inject(this string input, string stringToInject, int position)
        {
            if(position <= 0 || position >= input.Length)
            {
                return input;
            }

            var list = Enumerable.Range(0, input.Length / position)
                        .Select(m => input.Substring(m * position, position))
                        .ToList();

            return string.Join(stringToInject, list);
        }
    }
}
