using Newtonsoft.Json;
using RestSharp;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for AddJobsWindow.xaml
    /// </summary>
    public partial class AddJobsWindow : Window
    {
        int clientId = 0;
        public AddJobsWindow(int clientId)
        {
            this.clientId = clientId;
            InitializeComponent();
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

            job.client_job_id = clientId; //registered client

            var desc = "def func(var1, var2): return var1+var2"; // set user input here
            if (!String.IsNullOrEmpty(desc))
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(desc);
                job.description = Convert.ToBase64String(data);
            }
            else
            {
                MessageBox.Show("Null Description");
                job.description = "def func(a,b): return null";
            }
            //job.description = "def func(var1, var2): return var1+var2";
            job.name = nameBox.Text;
            RestClient restClient1 = new RestClient("http://localhost:9987/");
            RestRequest restRequest1 = new RestRequest("api/jobs/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(job));
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
            MessageBox.Show("Job Successfully Added!!!", Title = "Success Message");
            this.Close();
        }
    }
}
