using System;
using System.Threading.Tasks;
using RequestProcessor.App.Logging;
using RequestProcessor.App.Services;
using System.Linq;
using RequestProcessor.App.Exceptions;

namespace RequestProcessor.App.Menu
{
    /// <summary>
    /// Main menu.
    /// </summary>
    internal class MainMenu : IMainMenu
    {
        private readonly IOptionsSource _options;
        private readonly IRequestPerformer _performer;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor with DI.
        /// </summary>
        /// <param name="options">Options source</param>
        /// <param name="performer">Request performer.</param>
        /// <param name="logger">Logger implementation.</param>
        public MainMenu(
            IRequestPerformer performer, 
            IOptionsSource options, 
            ILogger logger)
        {
            _performer = performer;
            _options = options;
            _logger = logger;
        }

        public async Task<int> StartAsync()
        {
            try
            {
                var requestOptionsList = (await _options.GetOptionsAsync()).ToList();
                Console.WriteLine($"request options are successfully read from file!");
                var invalidRequestOptionsList = requestOptionsList.Where(op => !(op.Item1.IsValid && op.Item2.IsValid));
                foreach (var item in invalidRequestOptionsList)
                {
                    Console.WriteLine($"Request {item.Item1.Name ?? "no name"} is invalid and will not be performed!");
                    _logger.Log($"Request {item.Item1.Name ?? "no name"} is invalid and will not be performed!");
                }
                requestOptionsList = requestOptionsList.Where(op => op.Item1.IsValid && op.Item2.IsValid).ToList();

                var tasks = requestOptionsList.
                  Select(options => _performer.PerformRequestAsync(options.Item1, options.Item2)).ToArray();
             
                var t = await Task.WhenAll(tasks);
                Console.WriteLine($"Requests: " +
                    $"{string.Join(", ",requestOptionsList.Select(op => op.Item1.Name))}" +
                    " were successfully performed!");
            }
            catch (PerformException e)
            {
                Console.WriteLine("Unknown exception occured while performing requests!");
                _logger.Log(e, "PerformException was handled in main menu!");
                return -1;
            }
            return 0;
        }
    }
}
