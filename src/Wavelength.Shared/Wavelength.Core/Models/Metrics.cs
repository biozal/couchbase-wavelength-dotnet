using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wavelength.Core.Models
{
    public struct Metrics
    {
        public double NetworkLatency { get; set; }
        public double ApiLatency { get; set; }
        public double DbQueryExecutionTime { get; set; }
        public double DbQueryElapsedTime { get; set; }

        public IEnumerable<string> ToHeaders()
        {
            var headers = new List<string>();
            headers.Add($"netreq;desc=\"Net Req\";dur={NetworkLatency},");
            headers.Add($"dbqexec;desc=\"Query Exec\";dur={DbQueryExecutionTime},");
            headers.Add($"dbqelp;desc=\"Query Elapsed\";dur={DbQueryElapsedTime},");
            headers.Add($"api;desc=\"API\";dur={ApiLatency}");
            return headers;
        }
    }
}
