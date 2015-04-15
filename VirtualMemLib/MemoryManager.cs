using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    public class MemoryManager
    {
        static int FRAME_TABLE_SIZE = 16;
        private FrameTable _FrameTable;
        private LinkedList<Frame> _LRUStack;

        public MemoryManager()
        {
            _LRUStack = new LinkedList<Frame>();
            _FrameTable = new FrameTable(FRAME_TABLE_SIZE);
        }

        public FrameTable FrameTable
        {
            get
            {
                return _FrameTable;
            }
            private set
            {
                _FrameTable = value;
            }
        }

        /// <summary>
        /// Get the frame that goes with a particular page's process if it is in memory.
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="process"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Frame GetFrame(int frameIndex, string process, int page)
        {
            UpdateLRU(frameIndex, process, page);
            return _FrameTable[frameIndex];
        }

        private void UpdateLRU(int frameIndex, string process, int page)
        {
            LinkedListNode<Frame> node = _LRUStack.First;

            // Remove the frame from its place in the list (if it is in the list)
            while (node != null)
            {
                if (node.Value.Equals(process, page))
                {
                    _LRUStack.Remove(node);
                    break;
                }
                else
                {
                    node = node.Next;
                }
            }
            // Add the frame to the head of the list; it is now the most recently referenced page/frame
            _LRUStack.AddFirst(node);
        }
        
        public Frame PageFault(string process, int page, out int frameIndex)
        {
            Frame temp;

            //Get a free frame from the list (if available)
            if ((temp =_FrameTable.GetFreeFrame()) != null)
            {
                temp.Process = process;
                temp.Page = page;
                _LRUStack.AddFirst(temp);
                frameIndex = temp.FrameIndex;//_FrameTable.GetIndex(temp);
                return null;
            }
            //Otherwise choose a victim from the LRU stack
            string temp_proc;
            int temp_page;

            temp = _LRUStack.Last.Value;
            int frm_index = _FrameTable.GetIndex(temp);
            if (frm_index >= 0)
            {
                Console.WriteLine("Choosing a victim frame with index {0}", frm_index);
                Console.WriteLine("Process {0} page {1} generate the replacement", process, page);
                //Store the overwritten frame values so its process can update its page table
                temp_proc = temp.Process;
                temp_page = temp.Page;
                //Remove the replaced frame from the LRU Stack
                _LRUStack.Remove(temp);
                //Read the new page into the frame
                temp.Process = process;
                temp.Page = page;
                //Put newly read frame at the head of the LRU Stack
                _LRUStack.AddFirst(temp);
                //Make a new frame to return, this is used to update the overwritten process's frame table
                temp = new Frame();
                temp.Process = temp_proc;
                temp.Page = temp_page;
                frameIndex = frm_index;
                return temp;
            }
            frameIndex = -1;
            return null;

        }
        
    }
}
