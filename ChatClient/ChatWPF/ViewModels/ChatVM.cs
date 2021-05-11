using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWPF.Models;
using ChatWPF.Services;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ClientsList List { get; set; }

        public ChatVM()
        {
            var operations = new Operations(MainVM._navigationStore);
            var clientsList = operations.GetAllClients();

            List = new ClientsList();
            foreach (var item in clientsList)
            {
                List.AddClient(item);
            }
        }
    }
}
