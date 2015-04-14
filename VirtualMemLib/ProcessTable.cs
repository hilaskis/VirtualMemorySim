﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    public class ProcessTable
    {

        Dictionary<string, PCB> _Table;

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
        /// <returns></returns>
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
