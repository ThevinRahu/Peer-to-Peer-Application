using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        public bool FinishingJob(int id,JobPool jp)
        {

            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobpools/{id}", Method.Put);
            restRequest1.AddUrlSegment("id", id);
            restRequest1.AddJsonBody(JsonConvert.SerializeObject(jp));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);

            return true;

        }

        public Jobs downloadJobs(int id)
        {
            Jobs job = new Jobs();
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/jobs/{id}", Method.Get);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = restClient.Execute(restRequest);
            Jobs jobs = JsonConvert.DeserializeObject<Jobs>(restResponse.Content);
            return jobs;
        }

        public List<Jobs> connectServer(int id)
        {
            Jobs job = new Jobs();
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/getjobs/{id}", Method.Get);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Jobs> jobs = JsonConvert.DeserializeObject<List<Jobs>>(restResponse.Content);
            return jobs;
        }
    }
}
