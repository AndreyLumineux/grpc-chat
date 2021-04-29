using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWPF.Models;

namespace ChatWPF.ViewModels
{
    class HomeVM
    {
        public Label StatusLabel { get; set; }

        public HomeVM()
        {
            StatusLabel = new Label("Please enter your desired name");
        }
    }
}
