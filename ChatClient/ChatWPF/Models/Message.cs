using System.ComponentModel;

namespace ChatWPF.Models
{
    class Message : INotifyPropertyChanged
    {
        private string _outputMessage;

        public string OutputMessage
        {
            get => _outputMessage;
            set
            {
                _outputMessage = value;
                OnPropertyChanged("OutputMessage");
            }
        }

        public Message(string message)
        {
            var msg = " ";

            for (var i = 0; i < message.Length; i++)
            {
                if (message[i] == '*')
                {
                    var beginningFound = false;
                    if (i == 0)
                    {
                        beginningFound = true;
                    }
                    else if (message[i - 1] == ' ' && message[i + 1] != ' ')
                    {
                        beginningFound = true;
                    }
                    if (beginningFound)
                    {
                        var auxMessage = "<Bold>";
                        var endingFound = false;
                        for (var j = i + 1; j < message.Length; j++)
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
                    var beginningFound = false;
                    if (i == 0)
                    {
                        beginningFound = true;
                    }
                    else if (message[i - 1] == ' ' && message[i + 1] != ' ')
                    {
                        beginningFound = true;
                    }
                    if (beginningFound)
                    {
                        var auxMessage = "<Italic>";
                        var endingFound = false;
                        for (var j = i + 1; j < message.Length; j++)
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
                    var beginningFound = false;
                    if (i == 0)
                    {
                        beginningFound = true;
                    }
                    else if (message[i - 1] == ' ' && message[i + 1] != ' ')
                    {
                        beginningFound = true;
                    }
                    if (beginningFound)
                    {
                        var auxMessage = "<Strikethrough>";
                        var endingFound = false;
                        for (var j = i + 1; j < message.Length; j++)
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
                    var beginningFound = false;
                    if (i == 0)
                    {
                        beginningFound = true;
                    }
                    else if (message[i - 1] == ' ' && message[i + 1] != ' ')
                    {
                        beginningFound = true;
                    }
                    if (beginningFound)
                    {
                        var auxMessage = "<Underline>";
                        var endingFound = false;
                        for (var j = i + 1; j < message.Length; j++)
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
