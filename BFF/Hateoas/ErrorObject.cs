using Newtonsoft.Json;

namespace BFF.Hateoas
{
    public class ErrorObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }
}
