using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceConsole
{
    class ProcessRunner
    {
        private readonly Process process;

        public ProcessRunner(string command)
        {
            command = "cmd /c " + command + " 2>&1";
            process = new Process();
            string[] split = command.Split(' ');
            string[] args = new string[split.Length - 1];
            if (args.Length > 0) Array.Copy(split, 1, args, 0, args.Length);
            ProcessStartInfo startInfo = new ProcessStartInfo(split[0], string.Join(" ", args))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
        }

        public void start() {
            process.Start();
        }

        public StreamReader GetStdOut() {
            return process.StandardOutput;
        }

        public StreamWriter GetStdIn() {
            return process.StandardInput;
        }

        public StreamReader GetStdError() {
            return process.StandardError;
        }

        public Process GetProcess()
        {
            return process;
        }

        public void Exit()
        {
            process.WaitForExit();
        }

        public void Close() {
            try
            {
                process.StandardInput.WriteLine("stop");
                process.Kill();
            }
            catch (InvalidOperationException)
            { 
            }
            process.Close();
        }
    }
}
