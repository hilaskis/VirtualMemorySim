using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    public class Frame : INotifyPropertyChanged
    {
        private string _Process;
        private int _Page;
        private int _FrameIndex;

        
        public Frame()
        {
            this.Clear();
        }

        /// <summary>
        /// The current index of the frame in the frame table.
        /// </summary>
        public int FrameIndex
        {
            get { return _FrameIndex; }
           set 
            { 
                _FrameIndex = value;
                OnPropertyChanged("FrameIndex");
            }

        }

        /// <summary>
        /// Return the process that this frame currently holds a page for
        /// </summary>
        public string Process
        {
            get { return _Process; }
            set
            {
                _Process = value;
                OnPropertyChanged("Process");
            }
        }

        /// <summary>
        /// Return the page number that this frame current holds
        /// </summary>
        public int Page
        {
            get { return _Page; }
            set
            {
                _Page = value;
                OnPropertyChanged("Page");
            }
        }

        /// <summary>
        /// Clear the calling frame 
        /// </summary>
        public void Clear()
        {
            this.Process = String.Empty;
            this.Page = -1;
        }

        // Check if the frame is empty/clear
        public bool IsEmpty()
        {
            return ( this.Page < 0 && this.Process.Equals(String.Empty) );
        }

        //Simulate accessing the physical frame when referencing a page
        public void Access()
        {
            Console.WriteLine("Physical frame access for process {0} page {1}.", this.Process, this.Page);
        }

        /// <summary>
        /// Check if one frame is equal to another
        /// </summary>
        /// <param name="frm">Frame to perform equality comparison with</param>
        /// <returns>True if both frames are equal, otherwise false</returns>
        public bool Equals(Frame frm)
        {
            if (( this.Page == frm.Page ) && ( this.Process.Equals(frm.Process, StringComparison.OrdinalIgnoreCase) ))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the passed process string and page number match the contents of the frame
        /// </summary>
        /// <param name="process">Process name </param>
        /// <param name="page">Page number of the process</param>
        /// <returns>True if the frame contains the passed process's page number</returns>
        public bool Equals(string process, int page)
        {
            if (( this.Page == page ) && this.Process.Equals(process, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
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
