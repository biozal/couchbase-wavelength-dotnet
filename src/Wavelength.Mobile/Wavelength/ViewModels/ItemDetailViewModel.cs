using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Wavelength.Models;
using Wavelength.Repository;
using Xamarin.Forms;

namespace Wavelength.ViewModels
{
    [QueryProperty(nameof(Item), nameof(Item))]
    public class ItemDetailViewModel 
        : BaseViewModel
    {
        private ICBLiteAuctionRepository _auctionRepository;
        private AuctionItem _auctionItem; 
        
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
        
        public Command<IEnumerable<Bid>> BidItemsUpdateCommand { get;  }
        public ObservableCollection<Bid> Items { get; private set; }

        public ItemDetailViewModel()
        {
            Items = new ObservableCollection<Bid>();
            _auctionRepository = Startup.ServiceProvider.GetService<ICBLiteAuctionRepository>();
            
            //setup live query
            BidItemsUpdateCommand = new Command<IEnumerable<Bid>>(OnBidItemsUpdate);
        }
        
        public void OnDisappearing()
        {
            _auctionRepository.DeRegisterBidsLiveQuery(_auctionItem.DocumentId);       
        }
        
        private void LoadItem()
        {
            try
            {
                //todo - research better way to do this
                _auctionItem = JsonConvert.DeserializeObject<AuctionItem>(Item);
                
                //register for live query of bids
                if (_auctionItem is not null)
                {
                    _auctionRepository.RegisterBidsLiveQuery(BidItemsUpdateCommand, _auctionItem?.DocumentId);
                }
                
                //set UI items
                Text = _auctionItem.Title;
                ImageUrl = _auctionItem.ImageUrl;
                DisplayStopTime = $"Stop Time: {_auctionItem.StopTime.Humanize()}";
                
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        
        private void OnBidItemsUpdate(IEnumerable<Bid> bidItems)
        {
            IsBusy = true;
            //clear out previous items
            if (Items.Count > 0)
            {
                Items.Clear();
            }
            //add items
            foreach (var item in bidItems)
            {
                Items.Add(item);   
            }
            IsBusy = false;
        }
    }
}
