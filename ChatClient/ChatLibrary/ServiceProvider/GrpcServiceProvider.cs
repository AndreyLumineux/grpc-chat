using System;
using ChatProtos;
using Grpc.Net.Client;

namespace ChatLibrary.ServiceProvider
{
	public class GrpcServiceProvider
	{
		private static readonly GrpcServiceProvider instance;
		private Gateway.GatewayClient gatewayClient;
		private Message.MessageClient messageClient;

		public static GrpcServiceProvider Instance => instance ?? new GrpcServiceProvider();

		private string Url { get; set; }
		private Lazy<GrpcChannel> RpcChannel { get; set; }

		public Gateway.GatewayClient GatewayClient
		{
			get => gatewayClient ?? new Gateway.GatewayClient(RpcChannel.Value);
			set => gatewayClient = value;
		}

		public Message.MessageClient MessageClient
		{
			get => messageClient ?? new Message.MessageClient(RpcChannel.Value);
			set => messageClient = value;
		}

		private GrpcServiceProvider()
		{
			Url = "https://localhost:5001";
			RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(Url));
		}
	}
}