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
            string msg = " ";

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == '*')
                {
                    bool beginingFound = false;
                    if (i == 0)
                    {
                        beginingFound = true;
                    }
                    else if (message[i - 1] == ' ')
                    {
                        beginingFound = true;
                    }
                    if (beginingFound)
                    {
                        string auxMessage = "<Bold>";
                        bool endingFound = false;
                        for (int j = i + 1; j < message.Length; j++)
                        {
                            if (message[j] == '*' && j == message.Length - 1 && !endingFound)
                            {
                                auxMessage = auxMessage + "</Bold>";
                                endingFound = true;
                            }
                            else if (message[j] == '*' && message[j + 1] == ' ' && !endingFound)
                            {
                                auxMessage = auxMessage + "</Bold>";
                                endingFound = true;
                            }
                            else
                            {
                                auxMessage = auxMessage + message[j];
                            }
                        }
                        if (endingFound)
                            message = msg + auxMessage;
                        else
                            msg = msg + message[i];
                    }

                }
                else if (message[i] == '_')
                {
                    bool beginingFound = false;
                    if (i == 0)
                    {
                        beginingFound = true;
                    }
                    else if (message[i - 1] == ' ')
                    {
                        beginingFound = true;
                    }
                    if (beginingFound)
                    {
                        string auxMessage = "<Italic>";
                        bool endingFound = false;
                        for (int j = i + 1; j < message.Length; j++)
                        {
                            if (message[j] == '_' && j == message.Length - 1 && !endingFound)
                            {
                                auxMessage = auxMessage + "</Italic>";
                                endingFound = true;
                            }
                            else if (message[j] == '_' && message[j + 1] == ' ' && !endingFound)
                            {
                                auxMessage = auxMessage + "</Italic>";
                                endingFound = true;
                            }
                            else
                            {
                                auxMessage = auxMessage + message[j];
                            }
                        }
                        if (endingFound)
                            message = msg + auxMessage;
                        else
                            msg = msg + message[i];
                    }

                }
                else if (message[i] == '~')
                {
                    bool beginingFound = false;
                    if (i == 0)
                    {
                        beginingFound = true;
                    }
                    else if (message[i - 1] == ' ')
                    {
                        beginingFound = true;
                    }
                    if (beginingFound)
                    {
                        string auxMessage = "<Strikethrough>";
                        bool endingFound = false;
                        for (int j = i + 1; j < message.Length; j++)
                        {
                            if (message[j] == '~' && j == message.Length - 1 && !endingFound)
                            {
                                auxMessage = auxMessage + "</Strikethrough>";
                                endingFound = true;
                            }
                            else if (message[j] == '~' && message[j + 1] == ' ' && !endingFound)
                            {
                                auxMessage = auxMessage + "</Strikethrough>";
                                endingFound = true;
                            }
                            else
                            {
                                auxMessage = auxMessage + message[j];
                            }
                        }
                        if (endingFound)
                            message = msg + auxMessage;
                        else
                            msg = msg + message[i];
                    }

                }
                else if (message[i] == '`')
                {
                    bool beginingFound = false;
                    if (i == 0)
                    {
                        beginingFound = true;
                    }
                    else if (message[i - 1] == ' ')
                    {
                        beginingFound = true;
                    }
                    if (beginingFound)
                    {
                        string auxMessage = "<Underline>";
                        bool endingFound = false;
                        for (int j = i + 1; j < message.Length; j++)
                        {
                            if (message[j] == '`' && j == message.Length - 1 && !endingFound)
                            {
                                auxMessage = auxMessage + "</Underline>";
                                endingFound = true;
                            }
                            else if (message[j] == '`' && message[j + 1] == ' ' && !endingFound)
                            {
                                auxMessage = auxMessage + "</Underline>";
                                endingFound = true;
                            }
                            else
                            {
                                auxMessage = auxMessage + message[j];
                            }
                        }
                        if (endingFound)
                            message = msg + auxMessage;
                        else
                            msg = msg + message[i];
                    }
                }
                else
                {
                    i = msg.Length - 1;
                    msg = msg + message[i];
                }
            }

            _outputMessage = msg;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
