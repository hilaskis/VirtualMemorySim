using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics.Contracts;

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
       // private int _TableSize;         //
        #endregion

        public PCB(string procName, int tableSize)
        {
            ProcessID = procName;
            _PageTable = new Page[tableSize];
           // _TableSize = tableSize;
        }

        #region Properties

        public Page[] PageTable
        {
            get { return _PageTable; }
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
                _NumPages++;
            }

            return _PageTable[page];
        }

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
        /// Attempt to access the memory at a specified virtual memory address.
        /// </summary>
        /// <param name="virAddr">The virtual memory address</param>
        public void MemoryRef(int virAddr)
        {
            // Check for invalid address
            if (virAddr >= _PageTable.Length)
            {
                throw new IndexOutOfRangeException("Attempted access to address outside virtual memory size");
            }

            NumRef++;
            if (_PageTable[virAddr] == null)
            {
                NumPages++;
                _PageTable[virAddr] = new Page();
            }
        }

        public void PrintPageTable()
        {
            Console.WriteLine("Process {0} page table:", ProcessID);
            Console.WriteLine("Index\tFrame\tResident");
            for (int i = 0; i < _PageTable.Length; i++)
            {
                if (_PageTable[i] == null) return;
                Console.WriteLine("{0}\t{1}\t{2}", i, _PageTable[i].FrameIndex, _PageTable[i].Resident);
            }
        }

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
