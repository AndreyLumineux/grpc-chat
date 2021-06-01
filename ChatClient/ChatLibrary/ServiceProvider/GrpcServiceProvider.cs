using System;
using ChatProtos;
using Grpc.Net.Client;

namespace ChatLibrary.ServiceProvider
{
    public class GrpcServiceProvider
    {
        private static GrpcServiceProvider _instance;
        private Gateway.GatewayClient _gatewayClient;
        private Message.MessageClient _messageClient;

        public static GrpcServiceProvider Instance => _instance ??= new GrpcServiceProvider();

        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }

        public Gateway.GatewayClient GatewayClient
        {
            get => _gatewayClient ?? new Gateway.GatewayClient(RpcChannel.Value);
            set => _gatewayClient = value;
        }

        public Message.MessageClient MessageClient
        {
            get => _messageClient ?? new Message.MessageClient(RpcChannel.Value);
            set => _messageClient = value;
        }

        private GrpcServiceProvider()
        {
            Url = "https://localhost:5001";
            RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(Url));
        }
    }
}