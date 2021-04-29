using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ChatLibrary.ServiceProvider;
using ChatProtos;
using ChatWPF.ViewModels;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ChatWPF.Services
{
    class Operations
    {
        public void Submit(object param)
        {
            var invalidCharacters = "/-?!@#$%^&*() ";
            foreach (char character in invalidCharacters)
            {
                if (HomeVM.ClientName.Contains(character))
                {
                    HomeVM.StatusLabel.Message = "Your name contains an invalid character!";
                    HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
                    return;
                }
            }

            var response = CallGrpcService();
            if (response.Result.Status != ClientResponse.Types.Status.Success)
            {
                HomeVM.StatusLabel.Message = "Something went wrong while trying to connect to the server :(";
                HomeVM.StatusLabel.ForegroundColor = System.Windows.Media.Brushes.Red;
            }
            else
            {
                HomeVM.StatusLabel.Message = "Joining ...";
            }
        }

        private Task<ClientResponse> CallGrpcService()
        {
            var client = new GrpcServiceProvider().GetGatewayClient();
            ClientResponse result;
            try
            {
                result = client.InvokeAction(
                    new ClientRequest { Name = HomeVM.ClientName, Action = "connect" },
                    new CallOptions(deadline: DateTime.UtcNow.AddSeconds(2)));
            }
            catch
            {
                return Task.FromResult(new ClientResponse() { Status = ClientResponse.Types.Status.Error });
            }

            return Task.FromResult(result);
        }
    }


}
