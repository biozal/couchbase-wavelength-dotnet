using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Wavelength.Models;
using Wavelength.Views;
using Wavelength.Repository;
using Wavelength.Services;

namespace Wavelength.ViewModels
{
    public class InformationViewModel 
	   : BaseViewModel
    {
        private readonly ICBLiteDatabaseService _databaseService;
        private string _indexCount;
        public string IndexCount
        {
            get => _indexCount;
            set => SetProperty(ref _indexCount, value);
        }

        private string _datacenterLocation;
        public string DatacenterLocation
        {
            get => _datacenterLocation;
            set => SetProperty(ref _datacenterLocation, value);
        }

        private string _replicationStatus;
        public string ReplicationStatus
        {
            get => _replicationStatus;
            set => SetProperty(ref _replicationStatus, value);
        }

        private string _syncGatewayUri;

        public string SyncGatewayUri
        {
            get => _syncGatewayUri;
            set => SetProperty(ref _syncGatewayUri, value);
        }
        
        public Command DeleteDatabase { get; }
        
        public InformationViewModel(ICBLiteDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Settings";
            DeleteDatabase = new Command(OnDeleteDatabase);
            
            //default values
            IndexCount = "0";
            
            SetupFields();
        }

        private void SetupFields()
        {
            IndexCount = _databaseService.IndexCount;
            DatacenterLocation = _databaseService.DatacenterLocation;
            SyncGatewayUri = _databaseService.SyncGatewayUri;
        }
        
        private async void OnDeleteDatabase()
        {
            await Task.CompletedTask;
        }
    }
}
