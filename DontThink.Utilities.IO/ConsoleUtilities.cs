using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DontThink.Utilities.IO
{
    /// <summary>
    /// Helper methods for interaction with the Console.
    /// </summary>
    public static class ConsoleUtilities
    {
        /// <summary>
        /// Attempt to read a line from the console, outputting a message each time.
        /// The validation function is applied every time the user inputs a string, and should return true on a valid string.
        /// The failAction function will be invoked every time validation returns false.
        /// ReadLineRetry will attempt to read a line maxRetries number of times before returning null.
        /// </summary>
        /// <param name="message">Message output to console each try.</param>
        /// <param name="maxTries">Maximum number of tries the function will read from the console. Anything less than 0 will result in no retry limit.</param>
        /// <param name="validation">A function that is called with the user's input string each time. Return true or false to accept or reject the string.</param>
        /// <param name="failAction">A function that is called when validation rejects the string.</param>
        /// <returns>Whatever the user inputs after validation r#eturns true. null if the max number of retries is reached.</returns>
        /// <example>
        /// Try to get a user's name twice, then default to "user"
        /// <code>
        /// string name = ConsoleUtilities.ReadLineRetry(
        ///     message: "Enter name (under 10 characters)",
        ///     maxTries: 2,
        ///     validation: input =&gt; input.Length &lt; 10,
        ///     failAction: (input) =&gt; Console.WriteLine(
        ///         $"{input} is {input.Length} chracters which is more than 10, dummy...."))
        ///     ?? "user";
        /// 
        /// Console.WriteLine($"Your name is: {name}");
        /// </code>
        /// >Enter name(under 10 characters)
        /// >
        /// >thisnameistoolong
        /// >
        /// >thisnameistoolong is 17 chracters which is more than 10, dummy....
        /// >
        /// >Enter name(under 10 characters)
        /// >
        /// >thisnameisalsotoolong!
        /// >
        /// >thisnameisalsotoolong! is 22 chracters which is more than 10, dummy....
        /// >
        /// >Your name is: user
        /// </example>
        public static string ReadLineRetry(string message, int maxTries = -1, Func<string, bool> validation = null, Action<string> failAction = null)
        {
            string input;
            int tries = 0;
            while(maxTries < 0 || tries++ < maxTries)
            {
                Console.WriteLine(message ?? "");
                input = Console.ReadLine();

                if (validation?.Invoke(input) ?? string.IsNullOrEmpty(input))
                    return input;
                else
                    failAction?.Invoke(input);
            }
            return null;
        }


        /// <summary>
        /// Writes the result of ToString out to the console for each item in the colleciton.
        /// </summary>
        /// <param name="collection">Collection of objects to dump to the console.</param>
        /// <example>
        /// <code>
        /// WriteManyLines(new String[] { "one", "two", "three" });
        /// </code>
        /// > one
        /// 
        /// > two
        /// 
        /// > three
        /// </example>
        public static void WriteManyLines(IEnumerable collection)
        {
            foreach (var obj in collection)
                Console.WriteLine(obj);
        }

        /// <summary>
        /// Writes the result of ToString out to the console for each item in the colleciton.
        /// Will utilize the <see cref="CollectionBase.GetEnumerator"/> to enumerate the collection.
        /// </summary>
        /// <see cref="WriteManyLines(IEnumerable)"/>
        /// <param name="collection">Collection of objects to dump to the console.</param>
        public static void WriteManyLines(CollectionBase collection) => WriteManyLines(collection.Cast<object>());
    }
}
