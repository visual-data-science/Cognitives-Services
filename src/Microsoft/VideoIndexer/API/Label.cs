using System.Collections.Generic;

namespace VideoIndexer.Api
{
    public sealed class Label
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Time> Appearances { get; set; }
    }
}