using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWPF.ViewModels;

namespace ChatWPF.Stores
{
    public class NavigationStore
    {
        private BaseVM _currentVM;
        public BaseVM CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value;
                OnCurrentVMChanged();
            }
        }

        public event Action CurrentVMChanged;
        private void OnCurrentVMChanged()
        {
            CurrentVMChanged?.Invoke();
        }
    }
}
