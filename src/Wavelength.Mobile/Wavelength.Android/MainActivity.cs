﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Wavelength.Droid.Network;
using Wavelength.Services;
using Wavelength.Models;

namespace Wavelength.Droid
{
    [Activity(
	    Label = "Wavelength Demo", 
	    Icon = "@mipmap/icon", 
	    Theme = "@style/MainTheme", 
	    MainLauncher = true, 
	    ConfigurationChanges = 
	        ConfigChanges.ScreenSize | 
	        ConfigChanges.Orientation | 
	        ConfigChanges.UiMode | 
	        ConfigChanges.ScreenLayout | 
	        ConfigChanges.SmallestScreenSize)]
    public class MainActivity 
	    : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			var formsApp = new App();
#if DEBUG
			var handler = new HttpClientHandlerAndroidDebugFactory();
			var httpDataStore = new HttpDataStore(handler);
			Xamarin.Forms.DependencyService.RegisterSingleton<IDataStore<AuctionItem, AuctionItems>>(httpDataStore);
#endif
			formsApp.RegisterServices();
			LoadApplication(formsApp);
			global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
		}
        public override void OnRequestPermissionsResult(
	        int requestCode, 
	        string[] permissions, 
	        [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(
		        requestCode, 
		        permissions, 
				grantResults);
        }
    }
}