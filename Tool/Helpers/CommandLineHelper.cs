using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Helpers
{
    public static class CommandLineHelper
    {
        public static string ExecuteCommand(this List<string> cmdList)
        {

            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            foreach(var command in cmdList)
                cmd.StandardInput.WriteLine(command);

            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            return (cmd.StandardOutput.ReadToEnd());
        }
    }
}
