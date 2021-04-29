using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatLibrary.ServiceProvider;
using ChatProtos;

namespace ChatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _ = CallGrpcService();
        }

        private async Task CallGrpcService()
        {
            var client = new GrpcServiceProvider().GetChatClient();
            var reply = await client.SendMessageAsync(
                new SendRequest()
                {
                    Name = "george",
                    Message = "test"
                });
        }
    }
}
