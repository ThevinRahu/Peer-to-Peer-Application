using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    public class Clients : INotifyPropertyChanged
    {
        public Clients()
        {
            Id = 0;
            name = "";
            ip_address = "";
            port = 0;
        }

        private static Clients instance = null;
        public static Clients Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Clients();
                }
                return instance;
            }
        }

        public int Id { get; set; }
        public string name { get; set; }
        public string ip_address { get; set; }
        public Nullable<int> port { get; set; }

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
