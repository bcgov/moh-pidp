// namespace PlrIntake.Features.Intake;

// using Microsoft.AspNetCore.Http;
// using SoapCore.Extensibility;
// using SoapCore.ServiceModel;
// using System;
// using System.IO;
// using System.Security.Cryptography.X509Certificates;
// using System.Text;
// using System.Web;
// using System.Xml;
// using System.Xml.Linq;
// using System.Xml.XPath;

// public class IntakeServiceOperationTuner : IServiceOperationTuner
// {
//     public const string ClientCertHeader = "X-SSL-CERT";
//     public const string Prefix = "plr";
//     public const string Uri = "urn:hl7-org:v3";

//     private readonly string clientCertThumbprint;

//     public IntakeServiceOperationTuner(PlrIntakeConfiguration config) => this.clientCertThumbprint = config.ClientCertThumbprint;

//     public void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
//     {
//         if (serviceInstance is IntakeService service)
//         {
//             if (string.IsNullOrWhiteSpace(this.clientCertThumbprint))
//             {
//                 throw new InvalidOperationException("Receiving system is not configured properly; please advise system administrator.");
//             }

//             var requestCert = DecodeRequestCert(httpContext.Request, ClientCertHeader) ?? throw new ArgumentException("Client certificate not received.");

//             if (!requestCert.Thumbprint.Equals(this.clientCertThumbprint, StringComparison.OrdinalIgnoreCase))
//             {
//                 service.LogWarning($"A client provided an unrecognized certifcate with thumbprint {requestCert.Thumbprint}.");
//                 throw new ArgumentException("The provided certificate is invalid to the receiving system.");
//             }

//             service.DocumentRoot = GetRequestBody(httpContext, Prefix, Uri, operation.Name) ?? throw new ArgumentException("Could not resolve Request body into a namespaced XElement");
//         }
//     }

//     private static X509Certificate2? DecodeRequestCert(HttpRequest request, string headerName)
//     {
//         var header = request.Headers.TryGetValue(headerName, out var value)
//             ? value.SingleOrDefault()
//             : null;
//         if (header == null)
//         {
//             return null;
//         }

//         var decoded = HttpUtility.UrlDecode(header);
//         var certAsBytes = Encoding.ASCII.GetBytes(decoded);
//         return new X509Certificate2(certAsBytes);
//     }

//     private static XElement? GetRequestBody(HttpContext httpContext, string prefix, string uri, string bodyElement)
//     {
//         // Rewinding seems a bit expensive, but can't figure out why the stream
//         // is partially read at this point in the request, but it appears to
//         // occur in SoapCore as custom inlined middleware indicates it is not
//         // partially read prior to entering SoapCore's middleware
//         httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

//         // Produces a graph of XNode objects, which depending on our use case
//         // could be considered expensive with HL7 XML documents appearing to
//         // be quite large and deeply nested
//         var xDocument = XDocument.Load(httpContext.Request.Body);
//         return xDocument.XPathSelectElement($"//{prefix}:{bodyElement}", GetXmlNamespaceManager(prefix, uri));
//     }

//     private static XmlNamespaceManager GetXmlNamespaceManager(string prefix, string uri)
//     {
//         var xmlnsManager = new XmlNamespaceManager(new NameTable());
//         xmlnsManager.AddNamespace(prefix, uri);

//         return xmlnsManager;
//     }
// }
