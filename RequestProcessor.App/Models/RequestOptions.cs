using System;
using System.Text.Json.Serialization;

namespace RequestProcessor.App.Models
{
    class RequestOptions : IRequestOptions, IResponseOptions
    {
        [JsonPropertyName("path")]
        public string OptionalPath 
        { 
            get 
            { 
                return Path; 
            } 
            set 
            {
                Path = value.Replace('.', '_') + ".txt"; 
            } 
        }
        public string Path { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("method")]
        public string MethodAsString
        {
            get
            {
                return MethodAsString;
            }
            set
            {
                Method = Enum.TryParse<RequestMethod>(value, true, out RequestMethod r) ? r : RequestMethod.Undefined;
            }
        }

        public RequestMethod Method { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        public bool IsValid => Validate();

        private bool Validate()
        {
            return (
                !string.IsNullOrEmpty(Path) &&
                !(string.IsNullOrEmpty(ContentType) && !string.IsNullOrEmpty(Body)) &&
                Uri.IsWellFormedUriString(Address, UriKind.Absolute) &&
                !(Method == RequestMethod.Undefined) 
                );
        }
    }
}
