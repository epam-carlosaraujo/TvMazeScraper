namespace BFF.Hateoas
{
    public class UrlLink
    {
        public UrlLink(string rel, string href)
        {
            this.Href = href;
            this.Rel = rel;
        }

        public string Rel { get; set; }
        public string Href { get; set; }
    }
}
