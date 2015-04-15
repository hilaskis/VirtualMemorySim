using System;
using System.Text;
using System.ComponentModel;

namespace VirtualMemLib
{
    /// <summary>
    /// Class that represents a process control block for a proces
    /// </summary>
    public class PCB : INotifyPropertyChanged 
    {
        #region Member variables
        private uint _NumPages = 0;     // Represents the number of pages that a process has references.
        private uint _NumRef = 0;       // Represents the number of memory reference a process has made.
        private uint _NumFaults = 0;    // Represents the number of page faults encountered during process execution.
        private string _ProcessID;      // Represents the unique name of a process.
        private Page[] _PageTable;      // Page table with 64 entries
        #endregion

        public PCB(string procName, int tableSize)
        {
            ProcessID = procName;
            _PageTable = new Page[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                _PageTable[i] = new Page();
                _PageTable[i].PageNumber = i;
            }
        }

        #region Properties
        /// <summary>
        /// Property to get the page table
        /// </summary>
        public Page[] PageTable
        {
            get { return _PageTable; }
            private set { _PageTable = value; }
        }

        /// <summary>
        /// The total number of pages a process has referenced
        /// </summary>
        public uint NumPages
        {
            get
            {
                return _NumPages;
            }
            set
            {
                _NumPages = value;
                OnPropertyChanged("NumPages");
            }
        }

        /// <summary>
        /// The total number of memory references made by a process
        /// </summary>
        public uint NumRef
        {
            get
            {
                return _NumRef;
            }
            set
            {
                _NumRef = value;
                OnPropertyChanged("NumRef");
            }
            
        }

        /// <summary>
        /// The unique name of the process.
        /// </summary>
        public string ProcessID
        {
            get
            {
                return _ProcessID;
            }
            set
            {
                _ProcessID = value;
                OnPropertyChanged("ProcessID");
            }
            
        }

        /// <summary>
        /// The number of page faults that the process has generated during its runtime.
        /// </summary>
        public uint NumFaults
        {
            get
            {
                return _NumFaults;
            }
            set
            {
                _NumFaults = value;
                OnPropertyChanged("NumFaults");
            }
        }
        #endregion

        /// <summary>
        /// Check whether the specified page is already in physical memory.
        /// </summary>
        /// <param name="page"></param>
        /// <returns>True if the page is in memory. False otherwise.</returns>
        public Page GetPage(int page)
        {
            if(page >= _PageTable.Length) return null;

            //Add a page to the page table if it hasn't been referenced yet.
            if (_PageTable[page] == null)
            {
                _PageTable[page] = new Page();
                _PageTable[page].PageNumber = page;
            }

            // The page is new if the frame index is < 0
            if (_PageTable[page].FrameIndex < 0)
            {
                NumPages++;
            }

            return _PageTable[page];
        }

        /// <summary>
        /// Resets the resident bit of the specified page number in the process's page table to false.
        /// </summary>
        /// <param name="page">Page number to reset the resident bit of</param>
        public void ResetResident(int page)
        {
            if (page < 0 || page >= _PageTable.Length)
            {
                Console.WriteLine("PCB::ResetResident: Tried to access page outside virtual memory space");
            }

            _PageTable[page].Resident = false;
            OnPropertyChanged("PageTable");
        }


        /// <summary>
        /// Prints the entire contents of a process's page table to the console.
        /// </summary>
        public void PrintPageTable()
        {
            Console.WriteLine("---Process {0} Page Table---", ProcessID);
            Console.WriteLine("Index\tFrame\tResident");
            for (int i = 0; i < _PageTable.Length; i++)
            {
                if (_PageTable[i] == null || _PageTable[i].FrameIndex < 0) return;
                Console.WriteLine("{0}\t{1}\t{2}", i, _PageTable[i].FrameIndex, _PageTable[i].Resident);
            }
        }

        /// <summary>
        /// Returns a string with status information about the process.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buildStr = new StringBuilder();
            buildStr.AppendFormat("Process ID: {0}\n", _ProcessID);
            buildStr.AppendFormat("\tNumber of Pages: {0}\n", _NumPages);
            buildStr.AppendFormat("\tNumber of References: {0}\n", _NumRef);
            buildStr.AppendFormat("\tNumber of Page Faults: {0}\n", _NumFaults);
            return buildStr.ToString();
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
