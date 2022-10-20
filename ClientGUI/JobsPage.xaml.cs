using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using RestSharp;
using Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
using System.Windows.Threading;
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
            while (true)
            {
                Task<List<Clients>> task = new Task<List<Clients>>(updateGUIClients);

                task.Start();
                if (task.Wait(TimeSpan.FromSeconds(30)))
                {
                    List<Clients> clients = await task;

                    //display clients
                    if (clients.Count != 0)
                    {
                        List<Clients> gridData = new List<Clients>();
                        for (int i = 0; i <= clients.Count - 1; i++)
                        {
                            if (!String.IsNullOrEmpty(clients[i].ip_address))
                            {
                                byte[] data = Convert.FromBase64String(clients[i].ip_address);
                                clients[i].ip_address = System.Text.Encoding.UTF8.GetString(data);
                            }
                            gridData.Add(clients[i]);
                        }
                        servers.ItemsSource = gridData;
                    }
                    else
                    {
                        MessageBox.Show("No Available Servers!!");
                    }
                }
                else
                {
                    task.Dispose();
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
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
            while (true)
            {
                Task<List<Jobs>> task = new Task<List<Jobs>>(updateGUIJobs);

            task.Start();
            if (task.Wait(TimeSpan.FromSeconds(30)))
            {
                List<Jobs> jobs = await task;

                if (jobs.Count != 0)
                {
                    List<Jobs> gridData = new List<Jobs>();
                    for (int i = 0; i <= jobs.Count - 1; i++)
                    {
                            byte[] data = Convert.FromBase64String(jobs[i].description);
                            jobs[i].description = System.Text.Encoding.UTF8.GetString(data);
                            gridData.Add(jobs[i]);
                    }
                    job.ItemsSource =gridData;
                }
                else
                {
                    MessageBox.Show("No Available Jobs!!");
                }
            }
            else
            {
                task.Dispose();
            }
                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }
        public List<Jobs> updateGUIJobs()
        {
            InterfaceChannel iChannel = new InterfaceChannel();
            DataServerInterface iserverChannel;
            string URL = "net.tcp://"+Ip+ ":"+ 8100 +"/DataService";
            iserverChannel = iChannel.generateChannel(URL);
            List<Jobs> jobs = iserverChannel.connectServer(clientId); //server id/client_job_id
            return jobs;
        }

        Visual vis;
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

                    for (vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                        if (vis is DataGridRow) //Find current DataGridRow
                        {
                            clientId = selected.Id;
                            Ip = selected.ip_address;
                            Port = (int)selected.port;
                            userName.Content = "ID : " + selected.Id;
                            userToken.Content = "Name: " + selected.name;
                            ProgressChanged();
                            ConnectServer_Click();
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Selected Field is Empty", Title = "Empty Field Selected");
            }
        }

        private void ProgressChanged()
        {
            // Update the progressbar percentage only when the value is not the same.
            for (int i = 0; i <= 100; i++)
            {
                ((Clients)(((DataGridRow)vis).Item)).Progress = i;
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
                    for (vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                        if (vis is DataGridRow) //Find current DataGridRow
                        {
                            clientJobId = selected.client_job_id;
                            ProgressChanged2();
                            SelectJob_Click();
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Selected Field is Empty", Title = "Empty Field Selected");
            }
        }
        private void ProgressChanged2()
        {
            // Update the progressbar percentage only when the value is not the same.
            for (int i = 0; i <= 100; i++)
            {
                ((Jobs)(((DataGridRow)vis).Item)).Progress = i;
            }
        }
        private async void SelectJob_Click()
        {
            Task<Jobs> task = new Task<Jobs>(updateGUIJobSelected);

            task.Start();
            Jobs job = await task;
            try
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                var desc = "";
                if (!String.IsNullOrEmpty(job.description))
                {
                     byte[] data = Convert.FromBase64String(job.description);
                     desc = System.Text.Encoding.UTF8.GetString(data);
                    //desc = job.description;
                }
                else
                {
                    MessageBox.Show("Null Description");
                    desc = "def func(): return null";
                }
                engine.Execute(desc, scope);
                dynamic testFunction = scope.GetVariable("func");
                var result = testFunction(4, 4);
                Result.Text = result.ToString();
                result = result.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SelectJob_Click();
            }
            
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
            string URL = "net.tcp://" + Ip + ":" + 8100 + "/DataService";
            iserverChannel = iChannel.generateChannel(URL);
            Jobs jobs = iserverChannel.downloadJobs(clientId);
            return jobs;
        }

        
        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(finishJobSelected);

            task.Start();
            await task;
            for (int i = 1; i < 100; i++)
            {
                ProgressBar1.Dispatcher.Invoke(() => ProgressBar1.Value = i, DispatcherPriority.Background);
                Thread.Sleep(100);
            }
            MessageBox.Show("Successfully Uploaded!!!", Title = "Success Message");
        }

        public void finishJobSelected()
        {
            InterfaceChannel iChannel = new InterfaceChannel();
            DataServerInterface iserverChannel;
            string URL = "net.tcp://" + Ip + ":" + 8100 + "/DataService";
            iserverChannel = iChannel.generateChannel(URL);
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
