using System.Windows;
using ChatWPF.Services;

namespace ChatWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Operations operations = new Operations(null);
            operations.Close();
        }
    }
}
