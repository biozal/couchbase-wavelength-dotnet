using System;
using System.Net.Http;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using NBomber.Plugins.Network.Ping;

namespace Wavelength.Server.PerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using var httpClient = new HttpClient();
            var step = Step.Create("fetchAuctions", 
		                            clientFactory: HttpClientFactory.Create(),
				                    execute: context => { 
		        var request = Http.CreateRequest("GET", "https://localhost:9001/api/v1/Auction"); 
                return Http.Send(request, context);
            });

            var scenario = ScenarioBuilder
                            .CreateScenario("auctionsScenario", step)
                            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                            .WithLoadSimulations(
                                Simulation.InjectPerSec(rate: 250, during: TimeSpan.FromSeconds(30))
                            );

            // creates ping plugin that brings additional reporting data
            var pingPluginConfig = PingPluginConfig.CreateDefault(new[] { "localhost" });
            var pingPlugin = new PingPlugin(pingPluginConfig);

            NBomberRunner
                .RegisterScenarios(scenario)
                .WithWorkerPlugins(pingPlugin)
                .Run();
        }
	}
}
