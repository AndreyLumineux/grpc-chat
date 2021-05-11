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
        public override Task<GatewayResponse> Connect(GatewayRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name} has connected.");
            return Task.FromResult(new GatewayResponse
            {
                Name = request.Name,
                Status = GatewayResponse.Types.Status.Success
            });
        }

        public override Task<GatewayResponse> Disconnect(GatewayRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name} has disconnected.");
            return Task.FromResult(new GatewayResponse
            {
                Name = request.Name,
                Status = GatewayResponse.Types.Status.Success
            });
        }
    }
}
