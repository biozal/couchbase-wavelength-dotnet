using System;
namespace Wavelength.Constants
{
    public static class RestUri
    {
        public const string WavelengthServerProtocol = "https";
        public const string WavelengthServerBaseUrl = "192.168.50.225";
        public const int WavelengthServerPort = 9001;

        public const string WavelengthSyncGatewayProtocol = "ws";
        public const string WavelengthSyncGatewayUrl = "192.168.50.225";
        public const int WavelengthSyncGatewayPort = 4984;
        public const string WavelengthSyncGatewayEndpoint = "wavelength";


        public const string CloudServerProtocol = "https";
        public const string CloudServerBaseUrl = "wavelength-oregon.couchbase.live";
        public const int CloudServerPort = 443;

        public const string CloudSyncGatewayProtocol = "wss";
        public const string CloudSyncGatewayUrl = "wavelength-gateway-oregon.couchbase.live";
        public const int CloudSyncGatewayPort = 4984;
        public const string CloudSyncGatewayEndpoint = "wavelength";

        public const string SyncGatewayUsername = "demo";
        public const string SyncGatewayPassword = "password";

        public const string GetAuctions = "/api/v1/Auction";

    }
}
