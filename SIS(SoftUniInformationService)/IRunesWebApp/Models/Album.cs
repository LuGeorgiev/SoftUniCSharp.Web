
namespace IRunesWebApp.Models
{
    using System.Collections.Generic;

    public class Album:BaseEntity<string>
    {
        public string  Name { get; set; }

        public string  Cover { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<TrackAblum> Tracks { get; set; } = new HashSet<TrackAblum>();
    }
}
