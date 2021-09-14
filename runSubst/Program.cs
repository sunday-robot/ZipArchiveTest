using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runSubst
{
    class Program
    {
        static void Main(string[] args)
        {
            Subst('x', "c:/temp");
            Debug.WriteLine("substしました。");
            Subst('x');
            Debug.WriteLine("subst解除しました。");
        }

        static void Subst(char drive, string directoryPath)
        {
            var processStartInfo = new ProcessStartInfo("subst", $"{drive}: {directoryPath}");
            var process = Process.Start(processStartInfo);
            process.WaitForExit(5000);
        }

        static void Subst(char drive)
        {
            var processStartInfo = new ProcessStartInfo("subst", $"{drive}: /D");
            var process = Process.Start(processStartInfo);
            process.WaitForExit(5000);
        }
    }
}
