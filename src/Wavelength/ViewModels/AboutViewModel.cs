using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wavelength.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.verizon.com/business/solutions/5g/edge-computing/developer-resources/"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
