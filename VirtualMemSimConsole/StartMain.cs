using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VirtualMemLib;

namespace VirtualMemSimConsole
{
    class StartMain
    {
        static int NumPages = 64;
        static string InputFile = @"C:\Programs\VirtualMemSim\input3a.data";

        static void Main(string[] args)
        {
            OSKernel kernel = new OSKernel(InputFile);
            while (kernel.NextLine())
            {
                kernel.PrintCurrentState();
            }
            kernel.PrintPageTables();
        }
    }
}
