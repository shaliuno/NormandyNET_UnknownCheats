using System;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace NormandyNET.Helpers
{
    internal static class WebClientHelper
    {
        private static X509Certificate2 certificate;
        internal static HttpClient httpClient;
        internal static HttpClient httpClientSpecial;
        private static HttpClientHandler httpHandler;
        internal static string activeEndpoint;
        private static string certThumbprint = "DC18FEE88978D23908BEAC4BCA8848E1B1C7EC90";

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static void InitWebClientHelper_VMP()
        {
            httpHandler = new HttpClientHandler();
            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    certificate = new X509Certificate2(cert);

                    return certificate.Thumbprint.Equals(certThumbprint);
                };

            httpClientSpecial = new HttpClient(httpHandler);
            httpClient = new HttpClient();
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        internal static bool CheckSslCertificate_VMP(string strDNSEntry)
        {
            X509Certificate2 cert = null;
            using (TcpClient client = new TcpClient())
            {
                client.Connect(strDNSEntry, 5000);
                SslStream ssl = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate_VMP), null);

                try
                {
                    ssl.AuthenticateAsClient(strDNSEntry);
                    cert = new X509Certificate2(ssl.RemoteCertificate);
                    return cert.Thumbprint.Equals(certThumbprint);
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine(e.Message);
                    ssl.Close();
                    client.Close();
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ssl.Close();
                    client.Close();
                    return false;
                }
                ssl.Close();
                client.Close();
                return false;
            }
        }

        [System.Reflection.ObfuscationAttribute(Feature = "Virtualization", Exclude = false)]
        private static bool ValidateServerCertificate_VMP(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            var cert = new X509Certificate2(certificate);

            return cert.Thumbprint.Equals(certThumbprint);
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            return false;
        }
    }
}