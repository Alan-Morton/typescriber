using System.Diagnostics;

namespace ToTypeScript.Tests
{
    public static class TypeScriptCompiler
    {   
        public class Result
        {
            public int ReturnCode { get; private set; }

            public string Output { get; private set; }

            public string ErrorOutput { get; private set; }
                 
            public Result(int returnCode, string output, string errorOutput)
            {
                ReturnCode = returnCode;
                Output = output;
                ErrorOutput = errorOutput;
            }
        }
        
        public static Result CompileFiles(params string[] files)
        {
            var options = "";
            var process = new Process();
            process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), @"C:\Projects\Push.Playground\Push.Playground\ToTypeScript.Test\Tools\Microsoft.TypeScript.Compiler\tsc.exe");
            process.StartInfo.Arguments = string.Format("{0} {1}", options, string.Join(" ", files));
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit(10 * 1000);
            var output = process.StandardOutput.ReadToEnd();
            var errorOutput = process.StandardError.ReadToEnd();
            return new Result(process.ExitCode, output, errorOutput);
        }

        public static Result Compile(string typeScript)
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, typeScript);
            try
            {
                var newTempFile = tempFile.Replace(".tmp", ".ts");
                File.Move(tempFile, newTempFile);
                tempFile = newTempFile;
                return CompileFiles(tempFile);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
