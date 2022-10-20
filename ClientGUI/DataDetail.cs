using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    public class DataDetail : INotifyPropertyChanged
    {
        private int mProgress;
        public int Progress
        {
            get { return mProgress; }
            set
            {
                mProgress = value;
                OnPropertyChanged("Progress");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
