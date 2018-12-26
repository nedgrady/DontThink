using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontThink.Utilities.IO
{
    public class FileUtilities
    {
        /// <summary>
        /// Writes a specified string to a file in the directory where the program is running (as denoted by <see cref="Environment.CurrentDirectory"/>.
        /// </summary>
        /// <param name="str">The string to write to the file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="append">true to append data to the file; false to overwrite the file. 
        /// If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
        /// <exception cref="ArgumentNullException">null str or fileName provided</exception>
        /// <example>
        /// <code>
        /// await FileUtilities.WriteToCurrentDirectory("Overwriting everything in Out.txt");
        /// await FileUtilities.WriteToCurrentDirectory("Adding to the end of the file", "DontThink.txt", true);
        /// </code>
        /// </example>
        public static async Task WriteToCurrentDirectory(string str, string fileName = "Out.txt", bool append = true)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            using (StreamWriter writer = new StreamWriter(
                path: $@"{ Environment.CurrentDirectory }\{ fileName ?? throw new ArgumentNullException("fileName")}",
                append: append))
            {
                await writer.WriteLineAsync(str ?? "").ConfigureAwait(true);
            }

        }
    }
}
