using System.ComponentModel;

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
        private int _PageNumber;
        #endregion

        public Page()
        {
            _FrameIndex = -1;
            _Resident = false;
            _PageNumber = -1;
        }

        #region Properties
        /// <summary>
        /// The page number (index) of the page in a page table.
        /// </summary>
        public int PageNumber
        {
            get
            {
                return _PageNumber;
            }
            set
            {
                _PageNumber = value;
                OnPropertyChanged("PageNumber");
            }
        }

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
                OnPropertyChanged("FrameIndex");
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
                OnPropertyChanged("Resident");
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
