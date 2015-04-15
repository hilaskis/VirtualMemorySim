using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{

    public class FrameTable
    {
        private Frame[] _Table;
        private int _TableSize;

        public FrameTable(int tableSize)
        {
            _Table = new Frame[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                _Table[i] = new Frame();
            }
            _TableSize = tableSize;
        }

        public Frame[] Table
        {
            get { return _Table; }
            set { _Table = value; }
        }

        public Frame this[int index]
        {
            get
            {
                return _Table[index];
            }
            private set
            {
                _Table[index] = value;
            }
        }

        public int GetIndex(Frame frame)
        {
            for (int i = 0; i < _TableSize; i++)
            {
                if (_Table[i].Equals(frame))
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// Determine if the frame table (physical memory) is full
        /// </summary>
        /// <returns></returns>
        public Frame GetFreeFrame()
        {
            foreach (Frame frame in _Table)
            {
                if (frame.IsEmpty())
                {
                    return frame;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to remove a frame. If the frame is in the FrameTable the index of the 
        /// removed frames is returned. If the frame was not found, a -1 is returned.
        /// </summary>
        /// <param name="remFrame"></param>
        /// <returns>Index of removed frame or -1</returns>
        public int Remove(Frame remFrame)
        {
            for (int i = 0; i < _Table.Length; i++)
			{
                if (remFrame.Equals(this[i]))
                {
                    this[i].Page = -1;
                    this[i].Process = "";
                    return i;
                }
			}
            return -1;
        }

        //public int Contains(string proc, int page)
        //{
        //    for (int i = 0; i < _TableSize; i++)
        //    {
        //        if (Table[i].Process == proc && Table[i].Page == page)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

    }
}
