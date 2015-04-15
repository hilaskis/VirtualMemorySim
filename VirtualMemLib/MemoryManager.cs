using System;
using System.Collections.Generic;

namespace VirtualMemLib
{
    /// <summary>
    /// This class acts as the memory management unit for the kernel
    /// It handles page to frame conversion and the LRU replacement algorithm
    /// </summary>
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

        /// <summary>
        /// Property used to access the MMU's frame table
        /// </summary>
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
        /// <param name="frameIndex">The frame index of the stored page</param>
        /// <param name="process">The process that the stored page belongs to</param>
        /// <param name="page">The page number of the stored page within it's process's page table</param>
        /// <returns></returns>
        public Frame GetFrame(int frameIndex, string process, int page)
        {
            UpdateLRU(frameIndex, process, page);
            return _FrameTable[frameIndex];
        }

        /// <summary>
        /// Update the least recently used stack. Puts the most recently referenced page
        /// on the top of the stack.
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="process"></param>
        /// <param name="page"></param>
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
        
        /// <summary>
        /// Handle a page fault when trying to retrieve the specified page number of the specified process.
        /// The Frame object returned holds information about the replaced page, if any.
        /// The frameIndex represents the frame table index of the frame that was replaced.
        /// </summary>
        /// <param name="process">Process ID of the page request the generated the fault</param>
        /// <param name="page">The page number requested</param>
        /// <param name="frameIndex">The index of the frame that was replaced, if any</param>
        /// <returns>Frame that hold information about the replaced page. Returns null
        /// if no page was replaced.</returns>
        public Frame PageFault(string process, int page, out int frameIndex)
        {
            Frame frm;

            //Get a free frame from the list (if available)
            if ((frm =_FrameTable.GetFreeFrame()) != null)
            {
                frm.Process = process;
                frm.Page = page;
                _LRUStack.AddFirst(frm);    //Most recently referenced on top of stack
                frameIndex = frm.FrameIndex; 
                return null;
            }
            
            string temp_proc;
            int temp_page;

            frm = _LRUStack.Last.Value;     //Victim is the last frame in the stack
            int frm_index = _FrameTable.GetIndex(frm);
            //Otherwise choose a victim from the LRU stack
            if (frm_index >= 0)
            {
                Console.WriteLine("Choosing a victim frame with index {0}", frm_index);
                Console.WriteLine("The request for page {0} of process {1} generated the replacement\n", page, process);
                //Store the overwritten frame values so its process can update its page table
                temp_proc = frm.Process;
                temp_page = frm.Page;
                //Remove the replaced frame from the LRU Stack
                _LRUStack.Remove(frm);
                //Read the new page into the frame
                frm.Process = process;
                frm.Page = page;
                //Put newly read frame at the head of the LRU Stack
                _LRUStack.AddFirst(frm);
                //Make a new frame to return, this is used to update the overwritten process's frame table
                frm = new Frame();
                frm.Process = temp_proc;
                frm.Page = temp_page;
                frameIndex = frm_index;
                return frm;
            }
            
            frameIndex = -1;
            return null;

        }
        
    }
}
