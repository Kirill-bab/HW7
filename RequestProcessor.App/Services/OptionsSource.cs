using RequestProcessor.App.Models;
using System.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RequestProcessor.App.Services
{
    class OptionsSource : IOptionsSource
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };
        private readonly string _optionsPath;
        public OptionsSource(string optionsPath)
        {
            if (!File.Exists(optionsPath)) throw new ArgumentException(nameof(optionsPath));
            _optionsPath = optionsPath;
        }
        public async Task<IEnumerable<(IRequestOptions, IResponseOptions)>> GetOptionsAsync()
        {
            var requestOptionsList = (await JsonSerializer.
                   DeserializeAsync<List<RequestOptions>>(File.OpenRead(_optionsPath), _options)).
                    Select(op => ((IRequestOptions)op, (IResponseOptions)op));

            return requestOptionsList;     
        }
    }
}
