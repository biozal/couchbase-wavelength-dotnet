using System;
using System.Collections;
using System.Collections.Generic;
namespace Wavelength.Core.DataAccessObjects
{
    public class AuctionItemDAOs
    {
        public IEnumerable<AuctionItemDAO> Items { get; set; }
        public string DbQueryExecutionTime { get; set; } = string.Empty;
        public string DbQueryElapsedTime { get; set; } = string.Empty;
        public double ApiOverheadTime { get; set; }

        public AuctionItemDAOs(IEnumerable<AuctionItemDAO> items)
        {
            Items = items;
        }
    }
}
