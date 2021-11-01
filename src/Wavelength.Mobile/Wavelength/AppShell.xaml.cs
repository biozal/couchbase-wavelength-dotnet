using System;
using System.Collections.Generic;
using Wavelength.ViewModels;
using Wavelength.Views;
using Xamarin.Forms;

namespace Wavelength
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        }
    }
}
