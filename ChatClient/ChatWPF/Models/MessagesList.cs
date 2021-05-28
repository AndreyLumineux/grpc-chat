using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChatWPF.Models
{
    public class MessagesList : INotifyPropertyChanged
    {
        private ObservableCollection<string> _messages;
        public ObservableCollection<string> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public MessagesList()
        {
            Messages = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}