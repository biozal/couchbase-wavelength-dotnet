using System.Threading.Tasks;
using Wavelength.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wavelength.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            _viewModel = Startup.ServiceProvider.GetService<ItemsViewModel>(); 
            BindingContext = _viewModel;
        }

   

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
