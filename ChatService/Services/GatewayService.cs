using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatProtos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace ChatService.Services
{
    public class GatewayService : Gateway.GatewayBase
    {
        private static readonly List<IServerStreamWriter<GetClientsUpdateResponse>> ClientsResponseStreams = new();

        private readonly ConnectionService _connectionService = new();
        private readonly ILogger<GatewayService> _logger;

        public GatewayService(ILogger<GatewayService> logger)
        {
            _logger = logger;
        }

        public static List<string> ClientsList { get; set; } = new();

        public override async Task GetClientsUpdates(Empty request, IServerStreamWriter<GetClientsUpdateResponse> responseStream,
            ServerCallContext context)
        {
            if (!ClientsResponseStreams.Contains(responseStream))
            {
                ClientsResponseStreams.Add(responseStream);
            }

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }
        }

        public override async Task<ClientResponse> InvokeAction(ClientRequest request, ServerCallContext context)
        {
            var clientResponse = new ClientResponse
            {
                Status = ClientResponse.Types.Status.Error
            };

            switch (request.Action)
            {
                case "connect":
                    {
                        if (ClientsList.Contains(request.Name))
                        {
                            _logger.LogWarning($"{request.Name} tried to connect, but the username is already taken.");
                            break;
                        }

                        await _connectionService.Connect(new GatewayRequest { Name = request.Name }, context);
                        clientResponse.Status = ClientResponse.Types.Status.Success;
                        ClientsList.Add(request.Name);
                        _logger.LogInformation($"{request.Name} has connected!");
                        await SendClientsUpdateResponses(GetClientsUpdateResponse.Types.Status.Connected, request.Name);

                        break;
                    }
                case "disconnect":
                    {
                        await _connectionService.Disconnect(new GatewayRequest { Name = request.Name }, context);
                        clientResponse.Status = ClientResponse.Types.Status.Success;
                        ClientsList.Remove(request.Name);
                        _logger.LogInformation($"{request.Name} has disconnected!");
                        await SendClientsUpdateResponses(GetClientsUpdateResponse.Types.Status.Disconnected, request.Name);

                        break;
                    }
            }

            return await Task.FromResult(clientResponse);
        }

        private static Task SendClientsUpdateResponses(GetClientsUpdateResponse.Types.Status status, string name)
        {
            foreach (var responseStream in ClientsResponseStreams.ToList())
            {
                try
                {
                    responseStream.WriteAsync(new GetClientsUpdateResponse { Status = status, Client = name });
                }
                catch (Exception)
                {
                    ClientsResponseStreams.Remove(responseStream);
                }
            }

            return Task.CompletedTask;
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