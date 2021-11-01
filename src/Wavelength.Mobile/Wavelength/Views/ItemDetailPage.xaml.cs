using System.ComponentModel;
using Xamarin.Forms;
using Wavelength.ViewModels;

namespace Wavelength.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
