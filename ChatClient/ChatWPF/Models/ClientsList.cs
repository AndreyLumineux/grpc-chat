using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChatWPF.Models
{
	public class ClientsList : INotifyPropertyChanged
	{
		public ObservableCollection<string> Clients { get; set; }

		public ClientsList()
		{
			Clients = new ObservableCollection<string>();
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