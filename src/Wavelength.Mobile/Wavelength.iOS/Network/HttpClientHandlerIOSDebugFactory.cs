using System;
using System.Net.Http;
using Wavelength.Services;

namespace Wavelength.iOS.Network
{
    public class HttpClientHandlerIOSDebugFactory
        : IHttpClientHandlerFactory
    {
        public HttpClientHandlerIOSDebugFactory() { }

        public HttpClientHandler GetHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Contains("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
