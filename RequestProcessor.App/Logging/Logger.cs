using System;
using System.Diagnostics;

namespace RequestProcessor.App.Logging
{
    class Logger : ILogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public void Log(Exception exception, string message)
        {
            Debug.WriteLine(exception.GetType() + " was handled!");
            Debug.WriteLine(message);
        }
    }
}
