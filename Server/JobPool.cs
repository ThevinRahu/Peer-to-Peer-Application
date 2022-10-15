using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class JobPool
    {
        public int Id { get; set; }
        public int job_id { get; set; }
        public Nullable<int> finished { get; set; }
        public string solution { get; set; }
    }
}
