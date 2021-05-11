using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF.Models
{
    public class ClientsList : INotifyPropertyChanged
    {
        public List<string> Clients { get; set; }

        public ClientsList()
        {
            Clients = new List<string>();
        }

        public void AddClient(string client)
        {
            Clients.Add(client);
            OnPropertyChanged("Clients");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
