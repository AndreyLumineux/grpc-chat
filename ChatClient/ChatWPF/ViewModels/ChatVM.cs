﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using ChatWPF.Commands;
using ChatWPF.Models;
using ChatWPF.Services;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        private readonly object _lock = new();

        public ObservableCollection<string> Messages { get; } = new();
        public ObservableCollection<string> Clients { get; } = new();

        public Input InputBox { get; set; }

        public ChatVM()
        {
            Operations.CurrentContext = this;
            _ = Operations.Instance.Connect();

            var clientsList = Operations.Instance.GetAllClients();
            foreach (var item in clientsList)
            {
                Clients.Add(item);
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
                    _sendPressed = new RelayCommand(async param => await Operations.Instance.Send(InputBox.InputMessage));
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
                    _sendVoid = new RelayCommand(Operations.Instance.SendVoid);
                }

                return _sendVoid;
            }
        }
    }
}