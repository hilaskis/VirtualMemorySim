using System.Collections.Generic;
using System.Text;

namespace VirtualMemLib
{
    // Represents the process table used by the OS kernel
    public class ProcessTable
    {
        private Dictionary<string, PCB> _Table;

        public ProcessTable()
        {
            _Table = new Dictionary<string, PCB>();
        }

        /// <summary>
        /// The table of process control blocks
        /// </summary>
        public Dictionary<string, PCB> Table
        {
            get { return _Table; }
        }

        /// <summary>
        /// Print the contents of all processes' page tables
        /// </summary>
        public void PrintPageTables()
        {
            foreach(var key in _Table) 
            {
                key.Value.PrintPageTable();
            }
        }

        /// <summary>
        /// Returns a string representing the status of each process.
        /// </summary>
        /// <returns>String representation of the process table</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var key in _Table)
            {
                builder.AppendFormat("{0}", key.Value.ToString());
            }
            return builder.ToString();
        }
    }
}
