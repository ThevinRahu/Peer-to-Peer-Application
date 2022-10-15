using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetJobsController : ApiController
    {
        private clientsEntities3 db = new clientsEntities3();
        public IHttpActionResult GetJobs(int id)
        {
            List<Job> jobs = db.Jobs.Where(d => d.client_job_id == id).ToList();
            if (jobs == null)
            {
                return NotFound();
            }

            return Ok(jobs);
        }
    }
}
