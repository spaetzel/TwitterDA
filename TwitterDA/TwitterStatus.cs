using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaetzel.TwitterDA
{
    public class TwitterStatus
    {
        public DateTime CreatedAt { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public bool Truncated { get; set; }
    }
}
