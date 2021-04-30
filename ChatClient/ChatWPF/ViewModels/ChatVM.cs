using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWPF.Models;

namespace ChatWPF.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ClientsList List { get; set; }

        public ChatVM()
        {
            List = new ClientsList();
            List.AddClient(HomeVM.ClientName);
        }
    }
}
