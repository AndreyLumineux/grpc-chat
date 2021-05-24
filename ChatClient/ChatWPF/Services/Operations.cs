﻿using System;
using System.Collections.Generic;
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
		private NavigationStore _navigationStore;
		private static Message.MessageClient messageClient;

		public Operations(NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
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

			foreach (char character in invalidCharacters)
			{
				if (HomeVM.ClientName.Contains(character))
				{
					HomeVM.StatusLabel.LabelMessage = "Your name contains an invalid character!";
					HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
					return;
				}
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

			_navigationStore.CurrentVM = new ChatVM();
			Console.WriteLine("Successfully connected to server.");

			messageClient = GrpcServiceProvider.Instance.MessageClient;
			await ListenToServer(messageClient);
		}

		public async Task Send(string line)
		{
			await SendMessageToServer(line);
		}

		private static async Task SendMessageToServer(string text)
		{
			await messageClient.SendMessage()
				.RequestStream.WriteAsync(new ClientToServerMessage()
				{
					Name = HomeVM.ClientName,
					Text = text
				});


			// await _messageClient.SendMessage().RequestStream.CompleteAsync();
			Console.WriteLine("Sent message.");
		}

		private static async Task ListenToServer(Message.MessageClient messageClient)
		{
			await foreach (var response in messageClient.SendMessage().ResponseStream.ReadAllAsync())
			{
				Console.WriteLine($"Received: {response.Name} -- {response.Text}");
			}
		}

		private Task<ClientResponse> CallGrpcService(string action)
		{
			var client = GrpcServiceProvider.Instance.GatewayClient;
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
			var client = GrpcServiceProvider.Instance.GatewayClient;

			var result = client.GetAllClients(new Empty());
			return result.Clients == null ? new List<string>() : result.Clients;
		}
	}
}