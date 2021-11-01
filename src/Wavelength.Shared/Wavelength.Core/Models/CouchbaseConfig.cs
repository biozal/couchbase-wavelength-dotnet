using System;
namespace Wavelength.Core.Models
{
    public class CouchbaseConfig
    {
        public const string Section = "Couchbase";
        public const string ModeServer = "Server";
        public const string ModeCBLite = "CBLite";

        public bool UseSsl { get; set; } = true;
        public string? Mode { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConnectionString { get; set; }
        public string? RestEndpoint { get; set; }
        public string? BucketName { get; set; }
        public string? ScopeName { get; set; }
        public string? CollectionName { get; set; }
    }
}
