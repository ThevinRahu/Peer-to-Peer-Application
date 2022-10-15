using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class JobPoolsController : ApiController
    {
        private clientsEntities4 db = new clientsEntities4();

        // GET: api/JobPools
        public IQueryable<JobPool> GetJobPools()
        {
            return db.JobPools;
        }

        // GET: api/JobPools/5
        [ResponseType(typeof(JobPool))]
        public IHttpActionResult GetJobPool(int id)
        {
            JobPool jobPool = db.JobPools.Find(id);
            if (jobPool == null)
            {
                return NotFound();
            }

            return Ok(jobPool);
        }

        // PUT: api/JobPools/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutJobPool(int id, JobPool jobPool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jobPool.Id)
            {
                return BadRequest();
            }

            db.Entry(jobPool).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobPoolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/JobPools
        [ResponseType(typeof(JobPool))]
        public IHttpActionResult PostJobPool(JobPool jobPool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.JobPools.Add(jobPool);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (JobPoolExists(jobPool.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = jobPool.Id }, jobPool);
        }

        // DELETE: api/JobPools/5
        [ResponseType(typeof(JobPool))]
        public IHttpActionResult DeleteJobPool(int id)
        {
            JobPool jobPool = db.JobPools.Find(id);
            if (jobPool == null)
            {
                return NotFound();
            }

            db.JobPools.Remove(jobPool);
            db.SaveChanges();

            return Ok(jobPool);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JobPoolExists(int id)
        {
            return db.JobPools.Count(e => e.Id == id) > 0;
        }
    }
}