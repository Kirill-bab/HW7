using RequestProcessor.App.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    class ResponseHandler : IResponseHandler
    {
        public async Task HandleResponseAsync(IResponse response,
            IRequestOptions requestOptions,
            IResponseOptions responseOptions)
        {
            await using var writer = File.CreateText(responseOptions.Path);
            writer.Write("Status code: " + response.Code + Environment.NewLine);
            writer.Write("Is handled: " + response.Handled + Environment.NewLine);
            writer.Write("Content: " + Environment.NewLine);
            writer.Write(response.Content + Environment.NewLine);
            writer.Flush();
        }
    }
}
