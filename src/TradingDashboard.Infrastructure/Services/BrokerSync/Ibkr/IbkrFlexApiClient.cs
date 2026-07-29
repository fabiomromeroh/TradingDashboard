using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr;

namespace TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr
{

    public class IbkrFlexApiClient : IIbkrFlexApiClient
    {
        private readonly HttpClient _http;
        public static readonly HashSet<string> RetryableCodes = ["1001", "1004", "1009", "1019"];
        private readonly string _baseUrl;

        public IbkrFlexApiClient(HttpClient http, IOptions<IbkrFlexOptions> options)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("TradingDashboard/1.0");
            _baseUrl = options.Value.BaseUrl;

        }
        public async Task<XDocument> GetReportAsync(string referenceCode, string token, CancellationToken ct)
        {
            var url = QueryHelpers.AddQueryString(_baseUrl + "/GetStatement", new Dictionary<string, string?>
            {
                ["t"] = token,
                ["q"] = referenceCode,
                ["v"] = "3"
            });

            var response = await _http.GetStringAsync(url, ct);

            // A pending report still comes back as FlexStatementResponse XML with an error,
            // not the final <FlexQueryResponse> report, so check for that wrapper first.
            if (response.Contains("<FlexStatementResponse"))
            {
                var doc = XDocument.Parse(response);
                var code = doc.Root?.Element("ErrorCode")?.Value;
                var message = doc.Root?.Element("ErrorMessage")?.Value;
                throw new IbkrFlexException(code, message);
            }

            return XDocument.Parse(response);
        }

        public async Task<string> RequestReportAsync(string queryId, string token, string dateFrom, string dateTo, CancellationToken ct)
        {

            var url = QueryHelpers.AddQueryString(_baseUrl + "/SendRequest", new Dictionary<string, string?>
            {
                ["t"] = token,
                ["q"] = queryId,
                ["fd"] = dateFrom,
                ["td"] = dateTo,
                ["v"] = "3"
            });

            var xml = await _http.GetStringAsync(url, ct);
            var doc = XDocument.Parse(xml);

            var status = doc.Root?.Element("Status")?.Value;
            if (status != "Success")
            {
                var code = doc.Root?.Element("ErrorCode")?.Value;
                var message = doc.Root?.Element("ErrorMessage")?.Value;
                throw new IbkrFlexException(code, message);
            }

            return doc.Root!.Element("ReferenceCode")!.Value;
        }
    }


}
