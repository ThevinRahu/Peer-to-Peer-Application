using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Website.Models;

namespace Website.Controllers
{
    public class LoadController : Controller
    {
        [HttpGet]
        public IActionResult Reload()
        {
            //Clients client = new Clients();
            /*  RestClient restClient = new RestClient("http://localhost:9987/");
              RestRequest restRequest = new RestRequest("api/getclient/{ip}", Method.Get);
              restRequest.AddUrlSegment("ip", "0.0.0.0");
              RestResponse restResponse = restClient.Execute(restRequest);
              List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);
            */
            //Jobs job = new Jobs();
            /* RestClient restClient1 = new RestClient("http://localhost:9987/");
             RestRequest restRequest1 = new RestRequest("api/getjobs/{id}", Method.Get);
             restRequest1.AddUrlSegment("id", 1);
             RestResponse restResponse1 = restClient1.Execute(restRequest1);
             List<Jobs> jobs = JsonConvert.DeserializeObject<List<Jobs>>(restResponse1.Content); */
            int countConnect = 0;
            int countFinish = 0;
            //for (int i = 0; i < jobs.Count; i++)
            //{
            RestClient restClient2 = new RestClient("http://localhost:9987/");
            RestRequest restRequest2 = new RestRequest("api/jobpools", Method.Get);
            //restRequest2.AddUrlSegment("id", jobs[i].Id);
            RestResponse restResponse2 = restClient2.Execute(restRequest2);
            List<JobPool> jobpools = JsonConvert.DeserializeObject<List<JobPool>>(restResponse2.Content);
            for (int j = 0; j < jobpools.Count; j++)
            {
                if (jobpools[j].finished == 1)
                {
                    countFinish = countFinish + 1;
                }
                else
                {
                    countConnect = countConnect + 1;
                }

            }

            //}
            Count c = new Count();
            c.countConnected = countConnect;
            c.countFinished = countFinish;

            return Ok(c);
        }
    }
}
