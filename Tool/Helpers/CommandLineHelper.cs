using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.Helpers
{
    public static class CommandLineHelper
    {
        //public static string ExecuteCommand(this List<string> cmdList)
        //{

        //    try
        //    {

        //        Process cmd = new Process();

        //        cmd.StartInfo.FileName = "cmd.exe";
        //        cmd.StartInfo.RedirectStandardInput = true;
        //        cmd.StartInfo.RedirectStandardOutput = true;
        //        cmd.StartInfo.CreateNoWindow = true;
        //        cmd.StartInfo.UseShellExecute = false;

        //        cmd.Start();

        //        foreach (var command in cmdList)
        //            cmd.StandardInput.WriteLine(command);

        //        cmd.StandardInput.Flush();
        //        cmd.StandardInput.Close();

        //        return (cmd.StandardOutput.ReadToEnd());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public static string ExecuteCommand(this string filePath, List<string> args = null)
        {
            var process = new Process();

            process.StartInfo.FileName = filePath;
          
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            if (args != null)
            {
                foreach (var arg in args)
                    process.StartInfo.Arguments = $"{process.StartInfo.Arguments} {arg}";
            }

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data); // Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                throw new Exception( " finished with exit code = " + process.ExitCode + ": " + message);
            }
        }
        private static string Format(string filename, string arguments)
        {
            return "'" + filename +
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
    }
}
