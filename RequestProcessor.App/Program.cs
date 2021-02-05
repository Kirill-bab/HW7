using System.Threading.Tasks;
using RequestProcessor.App.Menu;
using RequestProcessor.App.Services;
using System;

namespace RequestProcessor.App
{
    /// <summary>
    /// Entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <returns>Returns exit code.</returns>
        private static async Task<int> Main()
        {
            try
            {
                var responseHandler = new ResponseHandler();
                var requestHandler = new RequestHandler(new System.Net.Http.HttpClient()
                {
                    Timeout = TimeSpan.FromSeconds(100)
                });
                var logger = new Logging.Logger();

                var performer = new RequestPerformer(requestHandler, responseHandler, logger);
                var options = new OptionsSource("options.json");
                var mainMenu = new MainMenu(performer, options, logger);
                return await mainMenu.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Critical unhandled exception");
                Console.WriteLine(ex);
                return -1;
            }

        }
    }
}
