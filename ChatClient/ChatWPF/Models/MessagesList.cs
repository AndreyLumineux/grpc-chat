using System.Collections.Generic;
using System.ComponentModel;

namespace ChatWPF.Models
{
	public class MessagesList : INotifyPropertyChanged
	{
		public List<string> Messages { get; set; }

		public MessagesList()
		{
			Messages = new List<string>();
		}

		public void AddMessage(string message)
		{
			Messages.Add(message);
			OnPropertyChanged("Messages");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}