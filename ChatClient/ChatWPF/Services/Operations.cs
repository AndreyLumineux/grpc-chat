using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatLibrary.ServiceProvider;
using ChatProtos;
using ChatWPF.Stores;
using ChatWPF.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ChatWPF.Services
{
    public class Operations
    {
        public static BaseVM CurrentContext;
        public static Operations Instance => instance ??= new Operations(CurrentContext, NavigationStore);

        private static NavigationStore NavigationStore;
        private static Message.MessageClient messageClient;
        private static Gateway.GatewayClient gatewayClient;
        private static Operations instance;

        private IAsyncStreamReader<ServerToClientMessage> messagesResponseStream;
        private IAsyncStreamReader<GetClientsUpdateResponse> clientsUpdatesResponseStream;

        public Operations(BaseVM currentContext, NavigationStore navigationStore)
        {
            CurrentContext = currentContext;
            NavigationStore = navigationStore;
        }

        public async void Submit(object param)
        {
            if (HomeVM.ClientName.Length <= 3)
            {
                HomeVM.StatusLabel.LabelMessage = "Your name must be at least 4 characters long!";
                HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
                return;
            }

            var invalidCharacters = "/-?~`'\\\"|!@#$%^&*() ";

            if (invalidCharacters.Any(character => HomeVM.ClientName.Contains(character)))
            {
                HomeVM.StatusLabel.LabelMessage = "Your name contains an invalid character!";
                HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
                return;
            }

            HomeVM.StatusLabel.LabelMessage = "Joining ...";
            HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.OliveDrab;
            await Task.Delay(500);
            var response = CallGrpcService("connect");

            if (response.Result.Status != ClientResponse.Types.Status.Success)
            {
                HomeVM.StatusLabel.LabelMessage = "Something went wrong while trying to connect to the server :(";
                HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
                return;
            }

            CurrentContext = new ChatVM();
            NavigationStore.CurrentVM = CurrentContext;
        }

        public async Task Connect()
        {
            Console.WriteLine("Successfully connected to server.");

            messageClient = GrpcServiceProvider.Instance.MessageClient;
            gatewayClient = GrpcServiceProvider.Instance.GatewayClient;

            clientsUpdatesResponseStream = gatewayClient.GetClientsUpdates(new Empty()).ResponseStream;
            messagesResponseStream = messageClient.SendMessage().ResponseStream;

            await Task.WhenAll(ListenToMessages(), ListenToClientsUpdates());
        }

        public async Task Send(string line)
        {
            await SendMessageToServer(line);
        }

        public void SendVoid(object obj)
        {
            (CurrentContext as ChatVM)?.Messages.Add("gfdahadfh");
        }

        private static async Task SendMessageToServer(string text)
        {
            ((ChatVM)CurrentContext).InputBox.InputMessage = "";
            await messageClient.SendMessage()
                .RequestStream.WriteAsync(new ClientToServerMessage
                {
                    Name = HomeVM.ClientName,
                    Text = text
                });

            Console.WriteLine("Sent message.");
        }

        private async Task ListenToMessages()
        {
            var currentContext = CurrentContext as ChatVM;

            await foreach (var response in messagesResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"Received: {response.Name} -- {response.Text}");
                var message = new Models.Message(response.Text);
                try
                {
                    currentContext?.Messages.Add($"Received: {response.Name} -- {message.OutputMessage}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // TODO: Exception handling
                }
            }
        }

        private async Task ListenToClientsUpdates()
        {
            await foreach (var response in clientsUpdatesResponseStream.ReadAllAsync())
            {
                if (response.Status == GetClientsUpdateResponse.Types.Status.Connected)
                {
                    Console.WriteLine($"{response.Client} has connected.");
                    ((ChatVM)NavigationStore.CurrentVM).Clients.AddClient(response.Client);
                }
                else
                {
                    Console.WriteLine($"{response.Client} has disconnected.");
                    ((ChatVM)NavigationStore.CurrentVM).Clients.RemoveClient(response.Client);
                }
            }
        }

        private Task<ClientResponse> CallGrpcService(string action)
        {
            var client = GrpcServiceProvider.Instance.GatewayClient;
            ClientResponse result;

            try
            {
                result = client.InvokeAction(new ClientRequest { Name = HomeVM.ClientName, Action = action },
                    new CallOptions(deadline: DateTime.UtcNow.AddSeconds(2)));
            }
            catch
            {
                return Task.FromResult(new ClientResponse { Status = ClientResponse.Types.Status.Error });
            }

            return Task.FromResult(result);
        }

        public void Close()
        {
            var response = CallGrpcService("disconnect");
        }

        public IList<string> GetAllClients()
        {
            var client = GrpcServiceProvider.Instance.GatewayClient;

            var result = client.GetAllClients(new Empty());
            return (IList<string>)result.Clients ?? new List<string>();
        }
    }
}