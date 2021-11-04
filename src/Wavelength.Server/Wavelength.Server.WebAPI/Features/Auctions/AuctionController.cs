using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Wavelength.Server.WebAPI.Features.Auctions
{
	[ApiController]
	[Route("/api/v1/[controller]")]
	public class AuctionController : ControllerBase
	{
		private readonly ILogger<AuctionController> _logger;
		private readonly IMediator _mediator;
		private readonly IWebHostEnvironment _hostEnvironment;

		public AuctionController(
			ILogger<AuctionController> logger,
			IMediator medator,
			IWebHostEnvironment hostEnvironment)
		{
			_logger = logger;
			_mediator = medator;
			_hostEnvironment = hostEnvironment;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			[FromQuery] GetAuctionsQuery.RequestQuery requestQuery)
		{
			try
			{

				var stopWatch = new Stopwatch();
				stopWatch.Start();
				var response = await _mediator.Send(requestQuery);
				if (response != null)
				{
					stopWatch.Stop();
					response.ApiOverheadTime = stopWatch.Elapsed.TotalMilliseconds;
					return this.Ok(response);
				}
				return NotFound();
			} 
			catch (Exception ex) 
			{
				return DealWithErrors(ex);
			}
		}

		private IActionResult DealWithErrors(Exception ex) 
		{
			switch (_hostEnvironment.EnvironmentName)
			{
				case Constants.Environments.Development:
				case Constants.Environments.Staging:
				case Constants.Environments.UAT:
					return this.Problem(ex.StackTrace, null, null, ex.Message, null);
				default:
					return this.Problem();

			}
		}

	}
}
