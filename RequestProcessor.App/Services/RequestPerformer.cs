using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Models;
using System.Net.Http;
using RequestProcessor.App.Exceptions;

namespace RequestProcessor.App.Services
{
    /// <summary>
    /// Request performer.
    /// </summary>
    internal class RequestPerformer : IRequestPerformer
    {
        private IRequestHandler _requestHandler;
        private IResponseHandler _responseHandler;
        private ILogger _logger;
        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="requestHandler">Request handler implementation.</param>
        /// <param name="responseHandler">Response handler implementation.</param>
        /// <param name="logger">Logger implementation.</param>
        public RequestPerformer(
            IRequestHandler requestHandler, 
            IResponseHandler responseHandler,
            ILogger logger)
        {
            _requestHandler = requestHandler;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> PerformRequestAsync(
            IRequestOptions requestOptions, 
            IResponseOptions responseOptions)
        {
            var failMessage = $"request {requestOptions.Name} failed!";
            try
            {
                var response = await _requestHandler.HandleRequestAsync(requestOptions);
                await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
                _logger.Log($"request {requestOptions.Name} succesfully performed and saved to {responseOptions.Path}!");
            }
            catch (ArgumentNullException e)
            {
                _logger.Log(e, $"{failMessage} Parameter {e.ParamName} is null!");
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                _logger.Log(e, $"{failMessage} Parameter {e.ParamName} is not valid!");
                return false;
            }
            catch (Exception e)
            {
                if(e is HttpRequestException || e is TaskCanceledException)
                {
                    _logger.Log(e, $"Request {requestOptions.Name} is cancelled due to timeout!");
                    IResponse response = new Response(false, 408, "Request was not performed due to timeout.");
                    await _responseHandler.HandleResponseAsync(response, requestOptions, responseOptions);
                    return true;
                }
                _logger.Log(e, $"Request {requestOptions.Name} is cancelled due to unexpected exception!");
                throw new PerformException(e.Message);
            }
            return true;
        }
    }
}
