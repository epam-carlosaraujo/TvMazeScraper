namespace BFF.Hateoas
{
    public class ResourceObject<TObject>
    {
        public ResourceObject(string id, string type)
        {
            this.Type = type;
            this.Id = id;
        }

        public string Id { get; set; }
        public string Type { get; set; }
    }
}
