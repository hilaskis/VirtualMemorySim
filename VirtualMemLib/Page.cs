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
        private int _Frame;
        private bool _Valid;
        #endregion

        public Page()
        {
            _Frame = -1;
            _Valid = false;
        }

        #region Properties
        /// <summary>
        /// The physical memory frame where the page is located.
        /// </summary>
        public int Frame
        {
            get
            {
                return _Frame;
            }
            set
            {
                _Frame = value;
                OnPropertyChanged("Frame");
            }
        }

        /// <summary>
        /// Valid bit used to determine if page is already loaded into physical memory
        /// </summary>
        public bool Valid
        {
            get
            {
                return _Valid;
            }
            set
            {
                _Valid = value;
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
