using ChatWPF.Stores;

namespace ChatWPF.ViewModels
{
    public class MainVM : BaseVM
    {
        public static readonly NavigationStore NavigationStore = new();

        public BaseVM CurrentVM => NavigationStore.CurrentVM;

        public MainVM()
        {
            NavigationStore.CurrentVM = new HomeVM();

            NavigationStore.CurrentVMChanged += OnCurrentVMChanged;
        }

        private void OnCurrentVMChanged()
        {
            OnPropertyChanged(nameof(CurrentVM));
        }
    }
}
