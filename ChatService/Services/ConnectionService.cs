using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatProtos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ChatService.Services
{
    public class ConnectionService : Connection.ConnectionBase
    {
        public static List<string> ClientsList { get; set; }

        public ConnectionService()
        {
            ClientsList = new List<string>();
        }

        public override Task<GatewayResponse> Connect(GatewayRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name} has connected.");
            ClientsList.Add(request.Name);
            return Task.FromResult(new GatewayResponse
            {
                Name = request.Name,
                Status = GatewayResponse.Types.Status.Success
            });
        }

        public override Task<GatewayResponse> Disconnect(GatewayRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name} has disconnected.");
            ClientsList.Remove(request.Name);
            return Task.FromResult(new GatewayResponse
            {
                Name = request.Name,
                Status = GatewayResponse.Types.Status.Success
            });
        }

        public override Task<GetAllClientsResponse> GetAllClients(Empty request, ServerCallContext context)
        {
            var books = ClientsList;

            var response = new GetAllClientsResponse();
            response.Clients.AddRange(books);

            return Task.FromResult(response);
        }
    }
}
