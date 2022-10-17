using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetJobPoolsController : ApiController
    {
        private clientsEntities4 db = new clientsEntities4();
        public IHttpActionResult GetJobs(int id)
        {
            List<JobPool> jobpools = db.JobPools.Where(d => d.job_id == id).ToList();
            if (jobpools == null)
            {
                return NotFound();
            }

            return Ok(jobpools);
        }
    }
}
