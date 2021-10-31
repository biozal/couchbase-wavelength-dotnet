using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Wavelength.Models;
using Xamarin.Forms;

namespace Wavelength.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        public string Id { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _itemId;
        public string ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = value;
                LoadItemId(value);
            }
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


        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
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
