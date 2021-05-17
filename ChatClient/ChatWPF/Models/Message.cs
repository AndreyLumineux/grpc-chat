using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF.Models
{
    class Message : INotifyPropertyChanged
    {
        private string _outputMessage;

        public string OutputMessage
        {
            get { return _outputMessage; }
            set
            {
                _outputMessage = value;
                OnPropertyChanged("OutputMessage");
            }
        }

        public Message(string message)
        {
            string finalMessage = "";
            string[] words = message.Split();
            foreach (var word in words)
            {
                if (word[0] == '*' && word[word.Length - 1] == '*')
                {
                    finalMessage = finalMessage + " " + "<Bold>" + word.Substring(1,word.Length-2) + "</Bold>";
                }
                else if(word[0] == '_' && word[word.Length - 1] == '_')
                {
                    finalMessage = finalMessage + " " + "<Italic>" + word.Substring(1, word.Length - 2) + "</Italic>";
                }
                else if (word[0] == '~' && word[word.Length - 1] == '~')
                {
                    finalMessage = finalMessage + " " + "<Italic>" + word.Substring(1, word.Length - 2) + "</Italic>";
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
