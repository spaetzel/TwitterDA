using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaetzel.TwitterDA
{
    public class TwitterUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Url { get; set; }
        public bool Protected { get; set; }
        public TwitterStatus Status { get; set; }
    }
}
