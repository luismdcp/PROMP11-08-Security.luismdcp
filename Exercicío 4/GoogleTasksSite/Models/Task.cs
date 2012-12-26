using System;
using System.Collections.Generic;

namespace GoogleTasksSite.Models
{
    public class Task
    {
        public String Kind { get; set; }
        public String Id { get; set; }
        public String Etag { get; set; }
        public String Title { get; set; }
        public DateTime Updated { get; set; }
        public String SelfLink { get; set; }
        public String Parent { get; set; }
        public String Position { get; set; }
        public String Notes { get; set; }
        public DateTime Due { get; set; }
        public DateTime Completed { get; set; }
        public Boolean Deleted { get; set; }
        public Boolean Hidden { get; set; }
        public List<LinkEntry> Links { get; set; }
    }
}