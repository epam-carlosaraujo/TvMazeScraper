using System.Collections.Generic;

namespace Common.Types.Data
{
    public class Show
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Cast> Cast { get; set; }
    }
}
