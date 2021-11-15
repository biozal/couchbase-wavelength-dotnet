using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wavelength.Models;
using Wavelength.Services;

namespace Wavelength.Repository
{
    public class AuctionHttpRepository
        : IAuctionHttpRepository
    {
        private readonly HttpClient _httpClient;
        
        public AuctionHttpRepository(
	        Services.IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.GetHttpClient();
        }

        public async Task<AuctionItemsDao> GetItemsAsync(bool forceRefresh = false)
        {
            var uri = new Uri(Constants.RestUri.GetAuctions, UriKind.Relative);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            try
            {
                //test performance
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var response = await _httpClient
                    .GetAsync(request.RequestUri, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);
                stopWatch.Stop();

                //parse results
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<AuctionItemsDao>(json);
                    results.NetworkOverheadTime = stopWatch.Elapsed.TotalMilliseconds - results.ApiOverheadTime;
                    return results;
                }
            }
            catch (Exception ex)
            {
                //todo logging
                throw ex;
            }
            return new AuctionItemsDao();
        }
    }
}
