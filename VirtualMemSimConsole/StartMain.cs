using System;
using VirtualMemLib;

namespace VirtualMemSimConsole
{
    public class StartMain
    {
        static int NumPages = 64;
        static string InputFile = @"C:\Programs\VirtualMemSim\input3a.data";

        static void Main(string[] args)
        {
            OSKernel kernel = new OSKernel(InputFile);
            while (kernel.NextLine())
            {
                kernel.PrintFrameTable();
                kernel.PrintPageTables();
                Console.WriteLine("-------------------------------------------");
            }
            
            kernel.PrintCurrentState();
        }
    }
}
