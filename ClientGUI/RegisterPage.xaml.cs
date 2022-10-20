using Newtonsoft.Json;
using RestSharp;
using System;
using System.Buffers.Text;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        int clientId = 0;
        public RegisterPage()
        {
            InitializeComponent();
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

           
            if (nameBox.Text == "")
            {
                MessageBox.Show("Name Feilds cannot be empty!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }else if (ipBox.Text == "")
            {
                MessageBox.Show("Ip Address Feilds cannot be empty!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }else if (portBox.Text == "")
            {
                MessageBox.Show("Port Feilds cannot be empty!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                client.name = nameBox.Text;
                client.ip_address = Encode(ipBox.Text);
                client.port = (int)Int64.Parse(portBox.Text);

                RestClient restClient1 = new RestClient("http://localhost:9987/");
                RestRequest restRequest1 = new RestRequest("api/clients/", Method.Post).AddJsonBody(JsonConvert.SerializeObject(client));
                RestResponse restResponse1 = restClient1.Execute(restRequest1);
                //List<Clients> clients1 = JsonConvert.DeserializeObject<List<Clients>>(restResponse1.Content);
                // if the entered credentials already exist then this error message will be shown
                MessageBox.Show("User Registered Successfully!!!", Title = "Registration Successfull");
                JobsPage jbPage = new JobsPage(client.Id,client.name);
                this.NavigationService.Navigate(jbPage);
            }
        }

        public string Encode(string St)
        {
            if (String.IsNullOrEmpty(St)){
                return St;
            }
            byte[] Stbytes = Encoding.UTF8.GetBytes(St);
            return Convert.ToBase64String(Stbytes);
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJobsWindow reg = new AddJobsWindow(clientId);
            reg.Show();
        }
    }
}
