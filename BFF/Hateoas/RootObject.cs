using Newtonsoft.Json;
using System.Collections.Generic;

namespace BFF.Hateoas
{
    public class RootObject<TData>
    {
        public RootObject()
        {
            this.JsonApi = new JsonApiObject();
        }

        public RootObject(TData data)
            : this()
        {
            this.Data = data;
        }

        public RootObject(TData data, IEnumerable<UrlLink> links)
            : this()
        {
            this.Links = links;
            this.Data = data;
        }

        public RootObject(TData data, IEnumerable<UrlLink> links, dynamic meta)
            : this()
        {
            this.Meta = meta;
            this.Links = links;
            this.Data = data;
        }

        public RootObject(IEnumerable<ErrorObject> errors)
            : this()
        {
            this.Errors = errors;
        }

        public RootObject(IEnumerable<ErrorObject> errors, IEnumerable<UrlLink> links)
            : this()
        {
            this.Links = links;
            this.Errors = errors;
        }

        public RootObject(IEnumerable<ErrorObject> errors, IEnumerable<UrlLink> links, dynamic meta)
            : this()
        {
            this.Meta = meta;
            this.Links = links;
            this.Errors = errors;
        }

        public JsonApiObject JsonApi { get; set; }
        public TData Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ErrorObject> Errors { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Meta { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<UrlLink> Links { get; set; }
    }
}
