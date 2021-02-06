
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestProcessor.App.Models
{
    class Response : IResponse
    {
        public bool Handled { get; }
        public int Code { get; }
        public string Content { get; }

        public Response(bool handled, int code, string content)
        {
            Handled = handled;
            Code = code;
            Content = content;
        }
    }
}
