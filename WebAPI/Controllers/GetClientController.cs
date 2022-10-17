using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetClientController : ApiController
    {
        private clientsEntities db = new clientsEntities();
        public IHttpActionResult GetClient(string ip)
        {
            List<Client> client = db.Clients.Where(d => d.ip_address == ip).ToList();
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }
    }
}
