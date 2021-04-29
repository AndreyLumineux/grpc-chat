using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Task<ClientResponse> InvokeAction(ClientRequest request, ServerCallContext context)
        {
            ClientResponse clientResponse = new ClientResponse()
            {
                Status = ClientResponse.Types.Status.Error
            };

            switch (request.Action)
            {
                case "connect":
                    connectionService.Connect(new GatewayRequest { Name = request.Name }, context);
                    clientResponse.Status = ClientResponse.Types.Status.Success;
                    break;
                case "disconnect":
                    connectionService.Disconnect(new GatewayRequest { Name = request.Name }, context);
                    clientResponse.Status = ClientResponse.Types.Status.Success;
                    break;
            }

            return Task.FromResult(clientResponse);
        }
    }
}
