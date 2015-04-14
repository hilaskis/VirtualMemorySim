using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMemLib
{
    /// <summary>
    /// A entry into the Page table
    /// </summary>
    public class Page : INotifyPropertyChanged
    {
        #region Member Variables
        private int _FrameIndex;
        private bool _Resident;
        #endregion

        public Page()
        {
            _FrameIndex = -1;
            _Resident = false;
        }

        #region Properties
        /// <summary>
        /// The physical memory frame where the page is located.
        /// </summary>
        public int FrameIndex
        {
            get
            {
                return _FrameIndex;
            }
            set
            {
                _FrameIndex = value;
                OnPropertyChanged("Frame");
            }
        }

        /// <summary>
        /// Valid bit used to determine if page is already loaded into physical memory
        /// </summary>
        public bool Resident
        {
            get
            {
                return _Resident;
            }
            set
            {
                _Resident = value;
                OnPropertyChanged("Valid");
            }
        }
        #endregion



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
