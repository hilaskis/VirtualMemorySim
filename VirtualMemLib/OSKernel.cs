using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    public class OSKernel
    {
        static int NUM_PAGES = 64;
        private ProcessTable _ProcessTable;
        private MemoryManager _MMU;
        private StreamReader _Reader;
        private PCB _CurrentProcess;
        private Page _CurrentPage;

        public OSKernel(string inputFile)
        {
            _MMU = new MemoryManager();
            _ProcessTable = new ProcessTable();
            _Reader = new StreamReader(inputFile);
        }

        public bool NextLine()
        {
            string[] tokStr;
            char[] delimiters = {' ', '\t', ':'};
            string line;
            // Check if the end of the file stream has been reached
            if (_Reader.EndOfStream)
            {
                return false;
            }

            //Read the next line from the input file
            line = _Reader.ReadLine();
            //Split the line into process and page reference
            tokStr = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            //Add the the process to the process table if it is new
            AddProcess(tokStr[0]);

            MemReference(tokStr[0], Convert.ToInt32(tokStr[1],2));

            return true;
        }

        private void ResetResident(string process, int page)
        {
            PCB temp;
            try
            {
                _ProcessTable.Table.TryGetValue(process, out temp);
                temp.ResetResident(page);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("OSKernel::ResetResident: tried to access process({0}), which doesn't exist", process);
            }
            

        }

        private void MemReference(string process, int page)
        {
            
            // Check for an invalid process 
            if (!_ProcessTable.Table.ContainsKey(process))
            {
                Debug.Print("The process {0} does not exist in the process table\n", process);
                return;
            }
            // Check for invalid page number
            if (page >= NUM_PAGES)
            {
                Debug.Print("The page number {0} is outside the virtual address space\n", page);
                return;
            }
            // Set the current process to the process making a memory reference (kind of like a context switch)
            _CurrentProcess = _ProcessTable.Table[process];
            _CurrentPage = _CurrentProcess.GetPage(page);
            if (_CurrentPage == null)
            {
                Console.WriteLine("Failed to get page from page table");
                return;
            }


            Console.WriteLine("Process {0} Page {1} is being accessed", process, page);
            if (!_CurrentPage.Resident) //Major page fault
            {
                int frameIndex;
                _CurrentProcess.NumFaults++;
                Frame temp = _MMU.PageFault(process, page, out frameIndex);
                _CurrentPage.Resident = true;
                _CurrentPage.FrameIndex = frameIndex;
                if (temp != null)
                {
                    if (_ProcessTable.Table.ContainsKey(temp.Process))
                    {
                        _ProcessTable.Table[temp.Process].ResetResident(temp.Page);
                    }
                    else
                    {
                        Console.WriteLine("OSKernel::MemReference: Process {0} does not exist in process table", process);
                    }
                }
            }
            else
            {
                // Get the frame index of the page already in physical memory
                int frmIndex = _CurrentPage.FrameIndex;
                // Check if the page is in physical memory
                // Get the physical memory frame
                Frame frame = _MMU.GetFrame(frmIndex, process, page);
                frame.Access();
            }
            _CurrentProcess.NumRef++;
        }

        /// <summary>
        /// Add a new process to the process table if it is not already in the list.
        /// </summary>
        /// <param name="processName"></param>
        private void AddProcess(string processName)
        {
            PCB newProc;
            //Do nothing if the process is alredy in the process table
            if (_ProcessTable.Table.TryGetValue(processName, out newProc))
            {
                return;
            }
            else //Otherwise create a new PCB and add it to the process table
            {
                newProc = new PCB(processName, NUM_PAGES);
                _ProcessTable.Table.Add(processName, newProc);
            }
        }

        public void PrintPageTables()
        {
            _ProcessTable.PrintPageTables();
        }
        
        public void PrintCurrentState()
        {
            Console.WriteLine(_ProcessTable.ToString());
        }
    }
}
