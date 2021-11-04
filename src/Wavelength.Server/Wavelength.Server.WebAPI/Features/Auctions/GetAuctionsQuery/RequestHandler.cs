using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Wavelength.Core.DataAccessObjects;
using Wavelength.Server.WebAPI.Repositories;

namespace Wavelength.Server.WebAPI.Features.Auctions.GetAuctionsQuery
{
	public class RequestHandler
		: IRequestHandler<RequestQuery, AuctionItemDAOs>
	{
		private readonly ILogger<RequestHandler> _logger;
		private readonly IAuctionRepository _auctionRepository;

		public RequestHandler(
			IAuctionRepository auctionRepository,
			ILogger<RequestHandler> logger)
		{
			_auctionRepository = auctionRepository;
			_logger = logger;
		}

		public async Task<AuctionItemDAOs> Handle(
			RequestQuery request, 
			CancellationToken cancellationToken)
		{
			var daos = new List<AuctionItemDAO>();
			var items = await _auctionRepository.GetAuctionItems(request.Limit, request.Skip);
			//convert to data access object
			items.Items.ToList().ForEach(item => 
			{
				daos.Add(item.ToAuctionItemDAO());
			});
			var results = new AuctionItemDAOs(daos);
			results.DbQueryElapsedTime = items.DbQueryElapsedTime;
			results.DbQueryExecutionTime = items.DbQueryExecutionTime;
			return results;
		}
	}
}
