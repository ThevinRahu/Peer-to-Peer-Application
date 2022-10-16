using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using RestSharp;
using Server;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimpleTcpServer tcpServer;
        private DataServerInterface foob;
        string URL;
        string result;
        public MainWindow()
        {
            InitializeComponent();

            updateGUIClientsAsync();
            
        }

        public async void updateGUIClientsAsync()
        {
            Task<List<Clients>> task = new Task<List<Clients>>(updateGUIClients);

            task.Start();
            List<Clients> clients = await task;
          
                //display clients
          
        }

        public List<Clients> updateGUIClients()
        {
            

            //display clients
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);
            return clients;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Clients client = new Clients();
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);
            if (clients == null)
            {
                client.Id = 1;
            }
            if (clients.Count > 0)
            {
                client.Id = clients[clients.Count - 1].Id + 1;
            }
            else
            {
                client.Id = 1;
            }


            client.name = "Thevin";
            client.ip_address = "0.0.0.0";
            client.port = 8100;
            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/clients/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(client)); 
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            Jobs job = new Jobs();
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/jobs/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Jobs> jobs = JsonConvert.DeserializeObject<List<Jobs>>(restResponse.Content);
            if (jobs == null)
            {
                job.Id = 1;
            }
            if (jobs.Count > 0)
            {
                job.Id = jobs[jobs.Count - 1].Id + 1;
            }
            else
            {
                job.Id = 1;
            }


            job.client_job_id = 1; //registered client
            job.description = "def func(var1, var2): return var1+var2";
            job.name = "name";
            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobs/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(job));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
        }

        private async void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            Task<List<Jobs>> task = new Task<List<Jobs>>(updateGUIJobs);

            task.Start();
            List<Jobs> jobs = await task;
          
                
                for (int i = 0; i < jobs.Count; i++)
                {
                    Jobs.Items.Add(jobs[i].name + "-" + jobs[i].description);
                }
           

        }

        private async void SelectJob_Click(object sender, RoutedEventArgs e)
        {
            Task<Jobs> task = new Task<Jobs>(updateGUIJobSelected);

            task.Start();
            Jobs job = await task;

            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.Execute(job.description, scope);
            dynamic testFunction = scope.GetVariable("func");
            var result = testFunction(2, 2);
            Result.Text = result.ToString();
            result = result.ToString();
            JobPool jobPool = new JobPool();
            jobPool.job_id = 1; //job id
            jobPool.finished = 0;
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/jobpools/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<JobPool> jobpools = JsonConvert.DeserializeObject<List<JobPool>>(restResponse.Content);
            if (jobpools == null)
            {
                jobPool.Id = 1;
            }
            if (jobpools.Count > 0)
            {
                jobPool.Id = jobpools[jobpools.Count - 1].Id + 1;
            }
            else
            {
                jobPool.Id = 1;
            }
            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobpools/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(jobPool));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
        }
        public Jobs updateGUIJobSelected()
        {
            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            Jobs jobs = foob.downloadJobs(1);
            return jobs;
        }

        public List<Jobs> updateGUIJobs()
        {
            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            List<Jobs> jobs = foob.connectServer(1); //server id/client_job_id
            return jobs;
        }

        private async void Finish_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(finishJobSelected);

            task.Start();
            await task;
        }
        public void finishJobSelected()
        {
            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            JobPool jp = new JobPool();
            jp.Id = 1;
            jp.job_id = 1;
            jp.solution = "4";
            jp.finished = 1;
            foob.FinishingJob(1,jp);
        }
    }
}
