using System;

namespace GoogleTasksSite.Models
{
    public class TaskList
    {
        public String Kind { get; set; }
        public String Id { get; set; }
        public String Etag { get; set; }
        public String Title { get; set; }
        public DateTime Updated { get; set; }
        public String SelfLink { get; set; }
    }
}