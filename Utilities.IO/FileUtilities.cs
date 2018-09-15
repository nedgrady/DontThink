using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.IO
{
    public class FileUtilities
    {
        public static async Task WriteToCurrentDirectory(string str, string fileName = null, bool append = true)
        {
            using (StreamWriter writer = new StreamWriter(
                path: $@"{ Environment.CurrentDirectory }\{ fileName ?? "Out.txt"}",
                append: append))
            {
                await writer.WriteLineAsync(str ?? "");
            }

        }
    }
}
