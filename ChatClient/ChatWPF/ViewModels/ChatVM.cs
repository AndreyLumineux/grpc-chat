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
using Meziantou.Framework.WPF.Collections;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        private readonly Operations _operations;
        private readonly object _lock = new();

        public ObservableCollection<string> Messages { get; } = new();
        public ObservableCollection<string> Clients { get; } = new();

        public Input InputBox { get; set; }

        public ChatVM()
        {
            _operations = new Operations(this, MainVM._navigationStore);
            var clientsList = _operations.GetAllClients();
            InputBox = new Input();
        }

        private ICommand _sendPressed;
        public ICommand SendPressed
        {
            get
            {
                if (_sendPressed == null)
                {
                    _sendPressed = new RelayCommand(async param => await _operations.Send(InputBox.InputMessage));
                }
                return _sendPressed;
            }
        }

        private ICommand _sendVoid;
        public ICommand SendVoid
        {
            get
            {
                if (_sendVoid == null)
                {
                    _sendVoid = new RelayCommand(_operations.SendVoid);
                }
                return _sendVoid;
            }
        }
    }
}
