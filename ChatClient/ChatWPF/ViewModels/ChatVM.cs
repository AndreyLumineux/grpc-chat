using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ChatWPF.Commands;
using ChatWPF.Models;
using ChatWPF.Services;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        public ClientsList Clients { get; set; }
        public Input InputBox { get; set; }

        public ChatVM()
        {
            var operations = new Operations(MainVM._navigationStore);
            var clientsList = operations.GetAllClients();

            Clients = new ClientsList();
            foreach (var item in clientsList)
            {
                Clients.AddClient(item);
            }

            InputBox = new Input();
            AddMessage("test");
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        private ICommand _sendPressed;
        public ICommand SendPressed
        {
            get
            {
                if (_sendPressed == null)
                {
                    var operations = new Operations(MainVM._navigationStore);
                    _sendPressed = new RelayCommand(async param => await operations.Send(InputBox.InputMessage));
                }
                return _sendPressed;
            }
        }
    }
}
