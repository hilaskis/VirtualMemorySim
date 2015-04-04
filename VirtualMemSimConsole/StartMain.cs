using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VirtualMemSimConsole
{
    class StartMain
    {
        
        static void Main(string[] args)
        {
            String input_file = @"C:\Programs\VirtualMemSim\input3a.data";
            StreamReader reader = new StreamReader(input_file);
            String[] parsed_line;
            String line;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                parsed_line = line.Split(' ', '\t');
                Console.WriteLine("Page Number Accessed: {0}", parsed_line[1]);
            }
        }
    }
}
