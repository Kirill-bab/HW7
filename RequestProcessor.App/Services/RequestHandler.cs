using RequestProcessor.App.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    class RequestHandler : IRequestHandler
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public RequestHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResponse> HandleRequestAsync(IRequestOptions requestOptions)
        {
            if (requestOptions == null) throw new ArgumentNullException(nameof(requestOptions));          
            if (!requestOptions.IsValid) throw new ArgumentOutOfRangeException(nameof(requestOptions));
            
            using var content = new StringContent(requestOptions.Body ?? "");
            var response = new HttpResponseMessage();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(100));

            switch (requestOptions.Method)
            {
                case RequestMethod.Undefined:
                    throw new InvalidOperationException(nameof(requestOptions.Method));
                case RequestMethod.Get:
                    response = await _httpClient.GetAsync(requestOptions.Address, cts.Token);
                    break;
                case RequestMethod.Post:
                    response = await _httpClient.PostAsync(requestOptions.Address, content, cts.Token);
                    break;
                case RequestMethod.Put:
                    response = await _httpClient.PutAsync(requestOptions.Address, content, cts.Token);
                    break;
                case RequestMethod.Patch:
                    response = await _httpClient.PatchAsync(requestOptions.Address, content, cts.Token);
                    break;
                case RequestMethod.Delete:
                    response = await _httpClient.DeleteAsync(requestOptions.Address, cts.Token);
                    break;
                default:
                    break;
            }
            return new Response(true, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
