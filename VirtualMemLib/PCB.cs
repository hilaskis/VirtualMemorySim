using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    /// <summary>
    /// Class that represents a process control block for a proces
    /// </summary>
    public class PCB
    {
        private uint _NumPages = 0;     // Represents the number of pages for a process.
        private uint _NumRef = 0;       // Represents the number of memory reference a process has made.
        private string _ProcessName;

        public PCB(string procName)
        {
            _ProcessName = procName;
        }


        public uint NumPages
        {
            get { return _NumPages; }
            set { _NumPages = value; }
        }
        public uint NumRef
        {
            get { return _NumRef; }
            set { _NumRef = value; }
        }

        public string ProcessName
        {
            get { return _ProcessName; }
        }



    }
}
