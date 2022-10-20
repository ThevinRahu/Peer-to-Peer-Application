using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Jobs : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int client_job_id { get; set; }
        public string description { get; set; }
        public string name { get; set; }

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
