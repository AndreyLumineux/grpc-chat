using System;
using System.Threading.Tasks;
using ChatProtos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ChatService.Services
{
	public class MessageService : Message.MessageBase
	{
		private readonly ILogger<MessageService> _logger;

		public MessageService(ILogger<MessageService> logger)
		{
			_logger = logger;
		}

		public override async Task SendMessage(IAsyncStreamReader<ClientToServerMessage> requestStream,
			IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context)
		{
			var clientToServerTask = ClientToServerMessage(requestStream, context);

			var serverToClientTask = ServerToClientMessage(responseStream, context);

			await Task.WhenAll(clientToServerTask, serverToClientTask);
		}

		private static async Task ServerToClientMessage(IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context)
		{
			while (!context.CancellationToken.IsCancellationRequested)
			{
				await responseStream.WriteAsync(new ServerToClientMessage
				{
					Name = "TestName",
					Text = "TestText"
				});
			}
		}

		private static async Task ClientToServerMessage(IAsyncStreamReader<ClientToServerMessage> requestStream, ServerCallContext context)
		{
			while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
			{
				var message = requestStream.Current;
				Console.WriteLine($"Received: {message.Name}: {message.Text}");
			}
		}
	}
}