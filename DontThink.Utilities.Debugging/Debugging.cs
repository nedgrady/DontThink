using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DontThink.Utilities.Debugging
{
    public class Debugging
    {
        public static int CurrentLineNumber([CallerLineNumber] int lineNumber = 0) => lineNumber;

        public static string CallerMemberName([CallerMemberName] string memberName = null) => memberName;

        public static string CurrentLineNumber([CallerFilePath] string filePath = null) => filePath;

        public static string CurrentCodeInfo(
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null) => $"{filePath} {memberName} : Line {lineNumber}";
    }
}
