using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Navigation;
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
		private NavigationStore _navigationStore;

		public Operations(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
		}

		public async void Submit(object param)
		{
			if (HomeVM.ClientName.Length <= 3)
			{
				HomeVM.StatusLabel.Message = "Your name must be at least 4 characters long!";
				HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
				return;
			}

			var invalidCharacters = "/-?~`'\\\"|!@#$%^&*() ";

			foreach (char character in invalidCharacters)
			{
				if (HomeVM.ClientName.Contains(character))
				{
					HomeVM.StatusLabel.Message = "Your name contains an invalid character!";
					HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
					return;
				}
			}

			HomeVM.StatusLabel.Message = "Joining ...";
			HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.OliveDrab;
			await Task.Delay(500);
			var response = CallGrpcService("connect");

			if (response.Result.Status != ClientResponse.Types.Status.Success)
			{
				HomeVM.StatusLabel.Message = "Something went wrong while trying to connect to the server :(";
				HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
				return;
			}

			_navigationStore.CurrentVM = new ChatVM();
			Console.WriteLine("Successfully connected to server.");


			// TODO: Send messages to server from GUI
			await SendMessageToServer();

			// TODO: Get asynchronous updates from server
			ListenToServer();
		}

		private static async Task SendMessageToServer()
		{
			var messageClient = new GrpcServiceProvider().GetMessageClient();
			var line = Console.ReadLine();

			while (!string.Equals(line, "qw!", StringComparison.OrdinalIgnoreCase))
			{
				await messageClient.SendMessage()
					.RequestStream.WriteAsync(new ClientToServerMessage()
					{
						Name = HomeVM.ClientName,
						Text = line
					});
				line = Console.ReadLine();
			}

			await messageClient.SendMessage().RequestStream.CompleteAsync();
		}

		private static void ListenToServer()
		{
			var messageClient = new GrpcServiceProvider().GetMessageClient();

			var result = Task.Run(async () =>
			{
				while (await messageClient.SendMessage().ResponseStream.MoveNext())
				{
					var msg = messageClient.SendMessage().ResponseStream.Current;
					Console.WriteLine($"Received: {msg.Name} -- {msg.Text}");
				}
			});
		}

		private Task<ClientResponse> CallGrpcService(string action)
		{
			var client = new GrpcServiceProvider().GetGatewayClient();
			ClientResponse result;

			try
			{
				result = client.InvokeAction(new ClientRequest {Name = HomeVM.ClientName, Action = action},
					new CallOptions(deadline: DateTime.UtcNow.AddSeconds(2)));
			}
			catch
			{
				return Task.FromResult(new ClientResponse() {Status = ClientResponse.Types.Status.Error});
			}

			return Task.FromResult(result);
		}

		public void Close()
		{
			var response = CallGrpcService("disconnect");
		}

		public IList<string> GetAllClients()
		{
			var client = new GrpcServiceProvider().GetGatewayClient();

			var result = client.GetAllClients(new Empty());
			return result.Clients == null ? new List<string>() : result.Clients;
		}
	}
}