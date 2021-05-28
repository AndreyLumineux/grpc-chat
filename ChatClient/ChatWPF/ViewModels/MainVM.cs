using ChatWPF.Stores;

namespace ChatWPF.ViewModels
{
    public class MainVM : BaseVM
    {
        public static readonly NavigationStore _navigationStore = new();

        public BaseVM CurrentVM => _navigationStore.CurrentVM;

        public MainVM()
        {
            _navigationStore.CurrentVM = new HomeVM();

            _navigationStore.CurrentVMChanged += OnCurrentVMChanged;
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }
    }
}
