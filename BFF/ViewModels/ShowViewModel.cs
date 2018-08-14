using System.Collections.Generic;

namespace BFF.ViewModels
{
    public class ShowViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CastViewModel> Cast { get; set; }
    }
}
