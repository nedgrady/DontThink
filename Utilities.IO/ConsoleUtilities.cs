using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.IO
{
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Attempt to read a line from the console, outputting a message each time.
        /// The validation function is applied every time the user inputs a string, and should return true on a valid string.
        /// The failAction function will be invoked every time validation returns false.
        /// ReadLineRetry will attempt to read a line maxRetries number of times before returning null.
        /// </summary>
        /// <param name="message">Message output to console each try</param>
        /// <param name="maxRetries">Maximum number of tries the function will read from the console. Anything less than 0 will result in no retry limit.</param>
        /// <param name="validation">A function that is called with the user's input string each time. Return true or false to accept or reject the string.</param>
        /// <param name="failAction">A function that is called when validation rejects the string.</param>
        /// <returns></returns>
        public static string ReadLineRetry(string message, int maxRetries = -1, Func<string, bool> validation = null, Action<string> failAction = null)
        {
            string input;
            int retries = 0;
            do
            {
                Console.WriteLine(message ?? "");
                input = Console.ReadLine();

                if (validation?.Invoke(input) ?? string.IsNullOrEmpty(input))
                    return input;
                else
                    failAction?.Invoke(input);

            } while (retries++ <= maxRetries || maxRetries < 0);

            return null;
        }
    }
}
