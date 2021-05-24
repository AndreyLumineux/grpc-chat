using System.Windows.Input;
using ChatWPF.Commands;
using ChatWPF.Models;
using ChatWPF.Services;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ClientsList Clients { get; set; }
        public MessagesList Messages { get; set; }
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

            Messages = new MessagesList();
            
            InputBox = new Input();
        }

        public void AddMessage(string message)
        {
            Messages.AddMessage(message);
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
