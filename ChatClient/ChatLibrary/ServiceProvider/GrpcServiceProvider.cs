using System;
using ChatProtos;
using Grpc.Net.Client;

namespace ChatLibrary.ServiceProvider
{
    public class GrpcServiceProvider
    {
        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }
        private Gateway.GatewayClient GatewayClient { get; set; }
        private Message.MessageClient MessageClient { get; set; }

        public GrpcServiceProvider()
        {
            Url = "https://localhost:5001";
            RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(Url));
        }

        public Gateway.GatewayClient GetGatewayClient() => GatewayClient ??= new Gateway.GatewayClient(RpcChannel.Value);
        public Message.MessageClient GetMessageClient() => MessageClient ??= new Message.MessageClient(RpcChannel.Value);
    }
}
