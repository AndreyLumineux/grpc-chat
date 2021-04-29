using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatProtos;
using Grpc.Net.Client;

namespace ChatLibrary.ServiceProvider
{
    public class GrpcServiceProvider
    {
        private string Url { get; set; }
        private Lazy<GrpcChannel> RpcChannel { get; set; }
        private Chat.ChatClient ChatClient { get; set; }

        public GrpcServiceProvider()
        {
            this.Url = "https://localhost:5001";
            this.RpcChannel = new Lazy<GrpcChannel>(GrpcChannel.ForAddress(this.Url));
        }

        public Chat.ChatClient GetChatClient() => this.ChatClient ??= new Chat.ChatClient(this.RpcChannel.Value);
    }
}
