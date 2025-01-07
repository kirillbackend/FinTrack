using FinTrack.Model;
using FinTrack.Services.Wrappers.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestSharp;

namespace FinTrack.Services.Wrappers
{
    public class FixerAPIWrapper : IFixerAPIWrapper
    {
        private readonly string _apiKey = "Tb0vi4hOYG3mr49vsaXNGdE2dNXhDDyn";
        private readonly string _baseFixerURL = "https://api.apilayer.com/fixer";
        private readonly RestClientOptions _options;
        private readonly IDistributedCache _cache;

        public FixerAPIWrapper(IDistributedCache cache) 
        {
            _options = new RestClientOptions(_baseFixerURL) { };
            _cache = cache;
        }

        public async Task<Symbol> GetSymbols()
        {
            var client = new RestClient(_options);
            var sympolsUrl = "/symbols";
            var request = await CreateBaseGetRequest(sympolsUrl);
            var response = client.Execute(request);

            var sumbols = await DeserializeResponse<Symbol>(response.Content);

            return sumbols;
        }

        public async Task<decimal> ConvertCurrency(string to, string from, string amount)
        {
            var cashKey = to + from + amount;
            var cash = await _cache.GetStringAsync(cashKey);

            if (cash == null)
            {
                var client = new RestClient(_options);
                var convertCurrencyUrl = $"/convert?to={to}&from={from}&amount={amount}";
                var request = await CreateBaseGetRequest(convertCurrencyUrl);
                var response = client.Execute(request);

                var currency = await DeserializeResponse<ConvertCurrency>(response.Content);

                await _cache.SetStringAsync(cashKey, currency.Result.ToString(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                return decimal.Parse(currency.Result.Replace(".", ","));
            }
            else
            {
                return decimal.Parse(cash.Replace(".", ","));
            }
        }

        public async Task LatestCurrency(string baseCurrency, List<string> symbols)
        {
            var client = new RestClient(_options);
            var symbol = string.Join(", ", symbols);
            var latestCurrencyUrl = $"/latest?symbols={symbol}&base={baseCurrency}";
            var request = await CreateBaseGetRequest(latestCurrencyUrl);
            var response = await client.ExecuteAsync(request);
        }

        #region private metods

        private async Task<RestRequest> CreateBaseGetRequest(string url)
        {
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("apikey", _apiKey);

            return request;
        }

        private async Task<T> DeserializeResponse<T>(string response)
        {
            var result = JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings { });

            return result;  
        }

        #endregion
    }
}
