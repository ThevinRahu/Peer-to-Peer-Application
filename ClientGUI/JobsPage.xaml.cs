using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using RestSharp;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.Xml;
using static IronPython.Modules._ast;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for JobsPage.xaml
    /// </summary>
    public partial class JobsPage : Page
    {
        int clientId =0;
        int clientJobId =0;
        int jobPoolId = 0;
        string Ip = "";
        int Port = 0;
        public JobsPage(int id, string name)
        {
            InitializeComponent();
            Clients user = Clients.Instance;
            userName.Content = "ID : " + id;
            clientId = id;
            userToken.Content = "Name: " + name;
            updateGUIClientsAsync();
        }

        public async void updateGUIClientsAsync()
        {
                Task<List<Clients>> task = new Task<List<Clients>>(updateGUIClients);

                task.Start();
                List<Clients> clients = await task;

                //display clients
                Clients clis;
                if (clients.Count != 0)
                {
                    List<Clients> gridData = new List<Clients>();
                    for (int i = 0; i <= clients.Count - 2; i++)
                    {
                        gridData.Add(clients[i]);
                    }
                    servers.ItemsSource = gridData;
                }
                else
                {
                    MessageBox.Show("No Available Servers!!");
                }
        }

        public List<Clients> updateGUIClients()
        {
            //display clients
            RestClient restClient = new RestClient("http://localhost:9987/");
            RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content.ToString());
            return clients;
        }

        private async void ConnectServer_Click()
        {
            Task<List<Jobs>> task = new Task<List<Jobs>>(updateGUIJobs);

            task.Start();
            List<Jobs> jobs = await task;

            if (jobs.Count != 0)
            {
                List<Jobs> gridData = new List<Jobs>();
                for (int i = 0; i <= jobs.Count - 1; i++)
                {
                    //Jobs.Items.Add(jobs[i].name + "-" + jobs[i].description);
                    gridData.Add(jobs[i]);
                }
                    job.ItemsSource =
                    gridData;
            }
            else
            {
                MessageBox.Show("No Available Jobs!!");
            }
        }
        public List<Jobs> updateGUIJobs()
        {
            InterfaceChannel iChannel = new InterfaceChannel();
            DataServerInterface iserverChannel;
            //string URL = "net.tcp://"+Ip+ ":"+ Port +"/DataService";
            iserverChannel = iChannel.generateChannel();
            List<Jobs> jobs = iserverChannel.connectServer(clientId); //server id/client_job_id
            return jobs;
        }

        private void clientPreview(object sender, RoutedEventArgs e)
        {
            Clients selected = servers.SelectedItem as Clients;
            if (selected != null)
            {
                if (selected.Id == 0)
                {
                    MessageBox.Show("Selected Feild is Empty", Title = "Empty Feild Selected");
                }
                else
                {
                    clientId = selected.Id;
                    Ip = selected.ip_address;
                    Port = (int)selected.port;
                    userName.Content = "ID : " + selected.Id;
                    userToken.Content = "Name: " + selected.name;
                    ConnectServer_Click();
                }
            }
            else
            {
                MessageBox.Show("Selected Field is Empty", Title = "Empty Field Selected");
            }
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJobsWindow reg = new AddJobsWindow(clientId);
            reg.Show();
        }

        private void jobPreview(object sender, RoutedEventArgs e)
        {
            Jobs selected = job.SelectedItem as Jobs;
            if (selected != null)
            {
                if (selected.client_job_id == 0)
                {
                    MessageBox.Show("Selected Feild is Empty", Title = "Empty Feild Selected");
                }
                else
                {
                    clientJobId = selected.client_job_id;
                    SelectJob_Click();
                }
            }
            else
            {
                MessageBox.Show("Selected Field is Empty", Title = "Empty Field Selected");
            }
        }

        private async void SelectJob_Click()
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
            jobPool.job_id = clientJobId; //job id
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
            jobPoolId = jobPool.Id;
            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobpools/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(jobPool));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
            
        }
        public Jobs updateGUIJobSelected()
        {
            InterfaceChannel iChannel = new InterfaceChannel();
            DataServerInterface iserverChannel;
            //string URL = "net.tcp://" + Ip + ":" + Port + "/DataService";
            iserverChannel = iChannel.generateChannel();
            Jobs jobs = iserverChannel.downloadJobs(clientId);
            return jobs;
        }

        
        private async void Finish_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(finishJobSelected);

            task.Start();
            await task;
            MessageBox.Show("Successfully Uploaded!!!", Title = "Success Message");
        }

        public void finishJobSelected()
        {
            InterfaceChannel iChannel = new InterfaceChannel();
            DataServerInterface iserverChannel;
            //string URL = "net.tcp://" + Ip + ":" + Port + "/DataService";
            iserverChannel = iChannel.generateChannel();
            JobPool jp = new JobPool();
            string srch = "";
            jp.Id = jobPoolId;
            jp.job_id = clientJobId;
            this.Dispatcher.Invoke((Action)(() =>
            {
                srch = Result.Text;
            }));
            jp.solution = srch;
            jp.finished = 1;
            iserverChannel.FinishingJob(jobPoolId, jp);
        }
    }
}
