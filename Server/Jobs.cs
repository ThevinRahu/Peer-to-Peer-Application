using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Jobs
    {
        public int Id { get; set; }
        public int client_job_id { get; set; }
        public int client_id { get; set; }
        public string description { get; set; }
    }
}
