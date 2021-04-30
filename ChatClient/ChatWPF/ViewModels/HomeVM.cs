﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatWPF.Commands;
using ChatWPF.Models;
using ChatWPF.Services;
using ChatWPF.Stores;

namespace ChatWPF.ViewModels
{
    class HomeVM : BaseVM
    {
        public static Label StatusLabel { get; set; }
        public static string ClientName { get; set; }

        public HomeVM()
        {
            StatusLabel = new Label("Please enter your desired name");
            ClientName = "defaultName";
        }

        private ICommand _joinPressed;
        public ICommand JoinPressed
        {
            get
            {
                if (_joinPressed == null)
                {
                    var operations = new Operations(MainVM._navigationStore);
                    _joinPressed = new RelayCommand(operations.Submit);
                }
                return _joinPressed;
            }
        }
    }
}
