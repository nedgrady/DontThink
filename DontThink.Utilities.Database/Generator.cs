using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace DontThink.Utilities.Database
{
    class Generator
    {
        static async Task Main()
        {
            string connStr = ErrorGet("C#GeneratorDatabase");
            string targetDB = ErrorGet("TargetDatabase");

            if (connStr == null || targetDB == null)
                return;

            string @namespace = MaybeGet("Namespace") ?? "Ned.Test";
            string appendText = MaybeGet("AppendText") ?? "THISSHOULDBEAGUID";
            string @class = MaybeGet("ClassName") ?? null;
            string outputFile = $@"{MaybeGet("OutputPath") ?? Environment.CurrentDirectory}\{targetDB}.cs";

            Console.WriteLine($@"Do you REALLY want to generate code using connection string {connStr}");
            Console.WriteLine($@"which will generate code for the target database {targetDB} into {outputFile}");
            Console.WriteLine("Y/N");


            if (Console.ReadKey().Key != ConsoleKey.Y)
                return;

            CSharpGeneratorDatabaseDatabase.CONNECTION_029C70EF556C469191E9C415EAE10DC0 = connStr;

            await CSharpGeneratorDatabaseDatabase.Execute_spC_GenerateAllMethods_dboAsync(
                @DBName: targetDB,
                @ConnectionString: connStr,
                @AppendText: appendText,
                @Namespace: @namespace,
                @ClassName: @class,
                callback: async  (SqlDataReader reader) =>
                {
                    while (await reader.ReadAsync())
                    {
                        File.WriteAllText(outputFile, reader[0].ToString());
                    }
                });

            Console.WriteLine();
            Console.WriteLine($"All done, check {outputFile}");
            Console.ReadLine();

        }

        static string MaybeGet(string s)
        {
            return ConfigurationManager.AppSettings[s]?.ToString() ??
                    ConfigurationManager.ConnectionStrings[s]?.ToString();
        }

        static string ErrorGet(string s)
        {
            string str = MaybeGet(s);

            if (str != null)
                return str;

            Console.WriteLine($"Set {s} in app.config fool");
            return null;
        }
    }
}
