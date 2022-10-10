using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Clients
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string ip_address { get; set; }
        public Nullable<int> port { get; set; }

    }
}
