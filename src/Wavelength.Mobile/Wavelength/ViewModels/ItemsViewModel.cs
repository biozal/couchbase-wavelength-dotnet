﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Wavelength.Models;
using Wavelength.Views;
using Wavelength.Repository;
using Newtonsoft.Json;

namespace Wavelength.ViewModels
{
    public class ItemsViewModel 
	    : BaseViewModel, IViewModel
    {
        private readonly IAuctionHttpRepository _auctionHttpRepository;
        private readonly ICBLiteAuctionRepository _auctionRepository;

        private AuctionItem _selectedItem;

        public ObservableCollection<AuctionItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<AuctionItem> ItemTapped { get; }

        public ItemsViewModel(ICBLiteAuctionRepository auctionRepository)
        {
            Title = "Auctions";

            _auctionRepository = auctionRepository;
             
            _auctionHttpRepository = DependencyService.Get<IAuctionHttpRepository>();
            Items = new ObservableCollection<AuctionItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<AuctionItem>(OnItemSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var results  = await _auctionHttpRepository.GetItemsAsync(true);
                foreach (var item in results.Items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public AuctionItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(AuctionItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.Item)}={JsonConvert.SerializeObject(item)}");
        }
    }
}
