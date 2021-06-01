using System.ComponentModel;

namespace ChatWPF.Models
{
    public class Input : INotifyPropertyChanged
    {
        private string _inputMessage;

        public string InputMessage
        {
            get => _inputMessage;
            set
            {
                _inputMessage = value;
                OnPropertyChanged("InputMessage");
            }
        }

        public Input()
        {
            _inputMessage = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
