using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatProtos;
using ChatService.Services;
using Google.Protobuf.WellKnownTypes;

namespace ChatService
{
	public class GatewayService : Gateway.GatewayBase
	{
		private readonly ILogger<GatewayService> _logger;

		public GatewayService(ILogger<GatewayService> logger)
		{
			_logger = logger;
		}

		private readonly ConnectionService connectionService = new();

		public static List<string> ClientsList { get; set; } = new();

		public override Task<ClientResponse> InvokeAction(ClientRequest request, ServerCallContext context)
		{
			ClientResponse clientResponse = new ClientResponse()
			{
				Status = ClientResponse.Types.Status.Error
			};

			switch (request.Action)
			{
				case "connect":
				{
					if (ClientsList.Contains(request.Name))
					{
						//TODO: use logger
						Console.WriteLine("A user with this name is already connected.");
						break;
					}

					connectionService.Connect(new GatewayRequest {Name = request.Name}, context);
					clientResponse.Status = ClientResponse.Types.Status.Success;
					ClientsList.Add(request.Name);
					break;
				}
				case "disconnect":
				{
					connectionService.Disconnect(new GatewayRequest {Name = request.Name}, context);
					clientResponse.Status = ClientResponse.Types.Status.Success;
					ClientsList.Remove(request.Name);
					break;
				}
			}

			return Task.FromResult(clientResponse);
		}

		public override Task<GetAllClientsResponse> GetAllClients(Empty request, ServerCallContext context)
		{
			var clients = ClientsList;

			var response = new GetAllClientsResponse();
			response.Clients.AddRange(clients);

			return Task.FromResult(response);
		}
	}
}