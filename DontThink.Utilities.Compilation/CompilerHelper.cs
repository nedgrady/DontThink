using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DontThink.Utilities.Compilation
{
    public static class CompilerHelper
    {
        public static CompilerResults CompileString(string sourceCode, string assemblyName = null)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters()
            {
                OutputAssembly = assemblyName,
                GenerateExecutable = false,
                GenerateInMemory = true
            };

            Regex r = new Regex("using .+", RegexOptions.Compiled);
            var matches = r.Matches(sourceCode);

            foreach (Match m in matches)
            {
                cp.ReferencedAssemblies.Add($"{m.Value.Trim().Remove(0, 5).Trim(';').Trim()}.dll");
            }
            return cpd.CompileAssemblyFromSource(cp, sourceCode);
        }
    }
}
