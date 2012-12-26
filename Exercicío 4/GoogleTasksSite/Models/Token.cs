using System;

namespace GoogleTasksSite.Models
{
    public class Token
    {
        public String Access_token { get; set; }
        public int Expires_in { get; set; }
        public String Token_type { get; set; }
    }
}