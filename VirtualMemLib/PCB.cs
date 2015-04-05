using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VirtualMemLib
{
    /// <summary>
    /// Class that represents a process control block for a proces
    /// </summary>
    public class PCB : INotifyPropertyChanged 
    {
        #region Member variables
        //private uint _NumPages = 0;     // Represents the number of pages for a process.
        //private uint _NumRef = 0;       // Represents the number of memory reference a process has made.
        //private string _ProcessName;    // Represents the unique name of a process.
        private Page[] _PageTable;  // Page table with 64 entries
        private int _TableSize;
        #endregion

        public PCB(string procName, int tableSize)
        {
            ProcessName = procName;
            _PageTable = new Page[tableSize];
            _TableSize = tableSize;
        }

        /// <summary>
        /// The number of total pages for a process
        /// </summary>
        public int NumPages
        {
            get;
            private set;
        }

        /// <summary>
        /// The total number of memory references made by a process
        /// </summary>
        public int NumRef
        {
            get;
            private set;
        }

        /// <summary>
        /// The unique name of the process (like a PID)
        /// </summary>
        public string ProcessName
        {
            get;
            private set;
        }

        /// <summary>
        /// Attempt to access the memory of a specified page number.
        /// </summary>
        /// <param name="page"></param>
        public void MemoryRef(int pageNum)
        {
            NumRef++;
            if (_PageTable[pageNum] == null)
            {
                NumPages++;
                _PageTable[pageNum] = new Page();
            }
        }

        public override string ToString()
        {
            StringBuilder buildStr = new StringBuilder();
            buildStr.AppendFormat("Process Name: {0}\n", ProcessName);
            buildStr.AppendFormat("Number of Pages: {0}\n", NumPages);
            buildStr.AppendFormat("Number of References: {0}\n", NumRef);
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
