using Xamarin.Forms;
using Wavelength.Services;
using Wavelength.Models;

namespace Wavelength
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        public void RegisterServices()
        {
#if DEBUG
            //used for dev debugging

#else
            DependencyService.Register<IHttpClientHandlerFactory, HttpClientHandlerFactory>();
#endif
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
