using System;
using Wavelength.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wavelength.Views
{
    public partial class InformationPage : ContentPage
    {
        private InformationViewModel _viewModel;

        public InformationPage()
        {
            InitializeComponent();
            _viewModel = Startup.ServiceProvider.GetService<InformationViewModel>();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Options_Tapped(
	                                    Object sender,
					                    EventArgs e)
        {
            var result = await DisplayPromptAsync(
		                                    "Admin Code", 
		                                    "Please enter the admin code to change options.");
        }

        private async void DeleteDatabase_Tapped( 
	                                    Object sender, 
					                    EventArgs e)
        {
            var shouldDelete = await DisplayAlert(
                    "Delete Database",
                    "Are you sure you want to delete the database?  You shouldn't do this unless someone from support instructs you to tap Yes.",
                    "Yes",
                    "No");

            if (shouldDelete)
            {
                _viewModel.DeleteDatabase.Execute(sender);
            }
        }
    }
}
