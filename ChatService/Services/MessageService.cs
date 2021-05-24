using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatProtos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ChatService.Services
{
	public class MessageService : Message.MessageBase
	{
		private static readonly List<IServerStreamWriter<ServerToClientMessage>> ResponseStreams = new();
		private readonly ILogger<MessageService> _logger;

		public MessageService(ILogger<MessageService> logger)
		{
			_logger = logger;
		}

		public override async Task SendMessage(IAsyncStreamReader<ClientToServerMessage> requestStream,
			IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context)
		{
			if (!ResponseStreams.Contains(responseStream))
			{
				ResponseStreams.Add(responseStream);
			}

			await ServerReceivedMessage(requestStream, context);
		}

		private async Task ServerSendMessage(IServerStreamWriter<ServerToClientMessage> responseStream, ServerCallContext context, string name,
			string text)
		{
			await responseStream.WriteAsync(new ServerToClientMessage
			{
				Name = name,
				Text = text
			});
		}

		private async Task ServerReceivedMessage(IAsyncStreamReader<ClientToServerMessage> requestStream, ServerCallContext context)
		{
			while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
			{
				var message = requestStream.Current;
				Console.WriteLine($"Received: {message.Name}: {message.Text}");

				foreach (var responseStream in ResponseStreams.ToList())
				{
					try
					{
						await ServerSendMessage(responseStream, context, message.Name, message.Text);
					}
					catch (Exception)
					{
						ResponseStreams.Remove(responseStream);
						Console.WriteLine(" ***** Removed a response stream. *****");
					}
				}
			}
		}
	}
}