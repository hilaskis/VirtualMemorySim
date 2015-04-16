using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

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

        /// <summary>
        /// The current process making a reference
        /// </summary>
        public PCB CurrentProcess
        {
            get { return _CurrentProcess; }
            set
            { 
                _CurrentProcess = value;
                OnPropertyChanged("CurrentProcess");
            }
        }

        /// <summary>
        /// Property used to access the memory management unit of the kernel
        /// </summary>
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


        /// <summary>
        /// Reads a line from the input file.
        /// </summary>
        /// <param name="tokens">The parsed parameters of the input line</param>
        /// <returns>False if the end of the file was reached. True otherwise.</returns>
        private bool ReadLine(out string[] tokens)
        {
            char[] delimiters = { ' ', '\t', ':' };
            string line;
            // Returns false if the end of the file has been reached
            if (_Reader.EndOfStream)
            {
                _Reader.Close();
                tokens = null;
                return false;
            }

            //Read the next line from the input file
            line = _Reader.ReadLine();
            //Split the line into process and page reference
            tokens = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            return true;
        }

        /// <summary>
        /// Reads from the file until the next page fault occurs.
        /// </summary>
        /// <returns>True if page fault was generated. False if end of file was reached.</returns>
        public bool NextFault()
        {
            string[] tokStr;
            do
            {
                if (!ReadLine(out tokStr) || tokStr == null)
                {
                    return false;
                }

                AddProcess(tokStr[0]);
            } while (MemReference(tokStr[0], Convert.ToInt32(tokStr[1], 2)) == false);

            return true;
        }

        /// <summary>
        /// Gets the next line from the input file and attempts to make a memory reference using
        /// the process and page specified in the line.
        /// </summary>
        /// <returns>False if the end of the file was reached or a blank line was encountered</returns>
        public bool NextLine()
        {
            string[] tokStr;

            if (!ReadLine(out tokStr) || tokStr == null)
            {
                return false;
            }

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
            PCB pcb;
            try
            {
                _ProcessTable.Table.TryGetValue(process, out pcb);
                pcb.ResetResident(page);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("OSKernel::ResetResident: tried to access process({0}), which doesn't exist", process);
            }
            

        }

        /// <summary>
        /// Request access to the page of the specified process.
        /// </summary>
        /// <param name="process">The process requesting a page</param>
        /// <param name="page">The page number to retrieve</param>
        /// <returns>True if the reference generated a fault.</returns>
        public bool MemReference(string process, int page)
        {
            // Check for an invalid process 
            if (!_ProcessTable.Table.ContainsKey(process))
            {
                Debug.Print("The process {0} does not exist in the process table\n", process);
                return false;
            }
            // Check for invalid page number
            if (page >= NUM_PAGES)
            {
                Debug.Print("The page number {0} is outside the virtual address space\n", page);
                return false;
            }
            // Set the current process to the process making a memory reference 
            CurrentProcess = _ProcessTable.Table[process];
            // Set the current page to the requested page
            _CurrentPage = CurrentProcess.GetPage(page);
            if (_CurrentPage == null)
            {
                Console.WriteLine("Failed to get page from page table");
                return false;
            }

            Console.WriteLine("Process {0} Page {1} is being accessed\n", process, page);

            // A page fault is generated if the page's resident value is false
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
                return true;
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
            return false;
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

        public void PrintFrameTable()
        {
            Console.WriteLine(_MMU.FrameTable);
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
