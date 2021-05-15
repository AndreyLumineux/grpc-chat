using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF.Models
{
    public class Label : INotifyPropertyChanged
    {
        private string _labelMessage;
        public string LabelMessage
        {
            get
            {
                return _labelMessage;
            }
            set
            {
                _labelMessage = value;
                OnPropertyChanged("LabelMessage");
            }
        }

        public Label(string labelMessage)
        {
            LabelMessage = labelMessage;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private System.Windows.Media.Brush _foregroundColor = System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                OnPropertyChanged("ForegroundColor");
            }
        }
    }
}
