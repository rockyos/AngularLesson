
using System.Collections.Generic;

namespace PhotoAPI.Models.Identity
{
    public class GoogleModel
    {
        public Dictionary<string,string>[] emails { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
    }
}
