using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatWPF.Commands;
using ChatWPF.Models;
using ChatWPF.Services;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ClientsList List { get; set; }
        public Input InputBox { get; set; }

        public ChatVM()
        {
            var operations = new Operations(MainVM._navigationStore);
            var clientsList = operations.GetAllClients();

            List = new ClientsList();
            foreach (var item in clientsList)
            {
                List.AddClient(item);
            }

            InputBox = new Input();
        }

        private ICommand _sendPressed;
        public ICommand SendPressed
        {
            get
            {
                if (_sendPressed == null)
                {
                    var operations = new Operations(MainVM._navigationStore);
                    _sendPressed = new RelayCommand(param => operations.Send(InputBox.InputMessage));
                }
                return _sendPressed;
            }
        }
    }
}
