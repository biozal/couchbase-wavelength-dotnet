using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using MediatR;
using Wavelength.Core.DataAccessObjects;
using System.Threading;

namespace Wavelength.Server.WebAPI.Features.Auction.CreateBidCommand
{
    public class RequestHandler
        : IRequestHandler<RequestCommand, BidDAO>
    {
        private readonly ILogger<RequestHandler> _logger;

        public RequestHandler(
            ILogger<RequestHandler> logger)
        {
            _logger = logger;
        }

        public async Task<BidDAO> Handle(
                RequestCommand request, 
                CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
