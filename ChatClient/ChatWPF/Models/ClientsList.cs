using System.Collections.Generic;
using System.ComponentModel;

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
			OnPropertyChanged(nameof(Clients));
		}

		public void RemoveClient(string client)
		{
			Clients.Remove(client);
			OnPropertyChanged(nameof(Clients));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}