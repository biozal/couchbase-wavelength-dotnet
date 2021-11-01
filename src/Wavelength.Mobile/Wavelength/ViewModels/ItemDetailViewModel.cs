using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Wavelength.Models;
using Xamarin.Forms;

namespace Wavelength.ViewModels
{
    [QueryProperty(nameof(Item), nameof(Item))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string _item;
        public string Item 
	    {   get => _item;
            set 
	        {
                _item = value;
                LoadItem();
	        } 
	    }

        public string Id { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

	    private string _displayStartTime;
	    public string DisplayStartTime 
	    {
	        get => _displayStartTime;
	        set => SetProperty(ref _displayStartTime, value);
	    }

        private string _displayStopTime;
        public string DisplayStopTime
        {
            get => _displayStopTime;
            set => SetProperty(ref _displayStopTime, value);
        }


        public void LoadItem()
        {
            try
            {
                var item = JsonConvert.DeserializeObject<AuctionItem>(Item);
                Text = item.Title;
                ImageUrl = item.ImageUrl;
                DisplayStartTime = $"Start Time: {item.StartTime.DateTime.ToShortTimeString()}";
                DisplayStopTime = $"Stop Time: {item.StopTime.DateTime.ToShortTimeString()}";
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
