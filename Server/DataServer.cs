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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {
        public bool FinishingJob(Jobs job)
        {

            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);
            if (clients == null)
            {
                job.Id = 1;
            }
            if (clients.Count > 0)
            {
                job.Id = clients[clients.Count - 1].Id + 1;
            }
            else
            {
                job.Id = 1;
            }

            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobs", Method.Post).AddJsonBody(JsonConvert.SerializeObject(job));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);

            return true;

        }

        public List<Clients> GetClientsRegistered()
        {

            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);
            return clients;
        }
    }
}
