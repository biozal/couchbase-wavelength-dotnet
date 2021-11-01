using System;
using System.Net.Http;
using Wavelength.Services;

namespace Wavelength.Droid.Network
{
    public class HttpClientHandlerAndroidDebugFactory
       : IHttpClientHandlerFactory
    {
        public HttpClientHandlerAndroidDebugFactory() { }

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
