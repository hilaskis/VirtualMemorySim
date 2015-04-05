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
            StreamReader reader = new StreamReader(InputFile);
            ProcessTable procTable = new ProcessTable();
            String[] parsed_line;
            String line;

            while (!reader.EndOfStream)
            {
                // Read a line from the input file
                line = reader.ReadLine();
                // Split the read line by tabs and spaces
                parsed_line = line.Split(' ', '\t');
                // If a new process was read, add it to the process table
                if (!procTable.Table.ContainsKey(parsed_line[0]))
                {
                    PCB newProc = new PCB(parsed_line[0], NumPages);
                    procTable.Table.Add(parsed_line[0], newProc);
                }

                procTable.Table[parsed_line[0]].MemoryRef(Convert.ToInt32(parsed_line[1],2));
                //Console.WriteLine("Page Number: {0}", Convert.ToInt32(parsed_line[1]));
               
            }
            Console.WriteLine(procTable.ToString());
        }
    }
}
