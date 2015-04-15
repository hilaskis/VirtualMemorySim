using System.Text;

namespace VirtualMemLib
{

    public class FrameTable
    {
        private Frame[] _Table;

        public FrameTable(int tableSize)
        {
            _Table = new Frame[tableSize];
            for (int i = 0; i < tableSize; i++)
            {
                _Table[i] = new Frame();
                _Table[i].FrameIndex = i;
            }
        }

        /// <summary>
        /// Property for the frame table
        /// </summary>
        public Frame[] Table
        {
            get { return _Table; }
            private set { _Table = value; }
        }

        /// <summary>
        /// Indexer for the frame table
        /// </summary>
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

        /// <summary>
        /// Finds the frame table index of the specified frame.
        /// </summary>
        /// <param name="frame">Positive frame index if it exists in the table, otherwise returns int < 0 </param>
        /// <returns></returns>
        public int GetIndex(Frame frame)
        {
            for (int i = 0; i < _Table.Length; i++)
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

        /// <summary>
        /// Returns a string representing the entire contents of the frame table.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder bldr = new StringBuilder();
            bldr.AppendFormat("---Physical Memory Frame Table---\n");
            bldr.AppendFormat("Frame#\tProcID\tPage#\n");
            foreach (var frm in _Table)
            {
                bldr.AppendFormat("{0}\t{1}\t{2}\n", frm.FrameIndex, frm.Process, frm.Page);
            }
            return bldr.ToString();
        }

    }
}
