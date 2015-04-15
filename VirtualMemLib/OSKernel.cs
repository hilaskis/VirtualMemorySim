using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VirtualMemLib
{
    public class OSKernel : INotifyPropertyChanged
    {
        static int NUM_PAGES = 64;
        private ProcessTable _ProcessTable;
        private MemoryManager _MMU;
        private StreamReader _Reader;
        private PCB _CurrentProcess;
        private Page _CurrentPage;

        public OSKernel(string inputFile)
        {
            MMU = new MemoryManager();
            _ProcessTable = new ProcessTable();
            _Reader = new StreamReader(inputFile);
        }

        public bool CanGetNextLine
        {
            get
            {
                return _Reader.EndOfStream;
            }

        }

        public PCB CurrentProcess
        {
            get { return _CurrentProcess; }
            set
            { 
                _CurrentProcess = value;
                OnPropertyChanged("CurrentProcess");
            }
        }

        public MemoryManager MMU
        {
            get
            {
                return _MMU;
            }
            private set
            {
                _MMU = value;
                OnPropertyChanged("MMU");
            }
        }

        public bool NextLine()
        {
            string[] tokStr;
            char[] delimiters = {' ', '\t', ':'};
            string line;
            // Returns false if the end of the file has been reached
            if (_Reader.EndOfStream)
            {
                _Reader.Close();
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

        /// <summary>
        /// Sets the resident flag of the given process's page to false
        /// </summary>
        /// <param name="process">The process the page belongs to</param>
        /// <param name="page">The page number/id</param>
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
            // Set the current process to the process making a memory reference 
            CurrentProcess = _ProcessTable.Table[process];
            // Set the current page to the requested page
            _CurrentPage = CurrentProcess.GetPage(page);
            if (_CurrentPage == null)
            {
                Console.WriteLine("Failed to get page from page table");
                return;
            }


            Console.WriteLine("Process {0} Page {1} is being accessed", process, page);
            // A major page fault is generated if the page's resident value is false
            if (!_CurrentPage.Resident)
            {
                int frameIndex;
                CurrentProcess.NumFaults++;
                // Returns the contents of the replaced frame and its index
                Frame temp = _MMU.PageFault(process, page, out frameIndex);
                // The current page is now residing in physical memory
                _CurrentPage.Resident = true;
                _CurrentPage.FrameIndex = frameIndex;
                // Set the resident bit to false for the replaced page's page table
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
                Frame frame = _MMU.GetFrame(frmIndex, process, page);
                // Simulate accessing the physical memory
                frame.Access();
            }
            CurrentProcess.NumRef++;
        }

        /// <summary>
        /// Add a new process to the process table if it is not already in the list.
        /// </summary>
        /// <param name="processName"></param>
        private void AddProcess(string processName)
        {
            PCB newProc;
            //Do nothing if the process is already in the process table
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

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
