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
    /// Interaction logic for ServerPage.xaml
    /// </summary>
    public partial class ServerPage : Page
    {
        public ServerPage()
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
                Clients clis;
                if (clients.Count != 0)
                {
                    List<Clients> gridData = new List<Clients>();
                    for (int i = 0; i <= clients.Count - 1; i++)
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterPage rjPage = new RegisterPage();
            this.NavigationService.Navigate(rjPage);
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
                    JobsPage jbPage = new JobsPage(selected.Id, selected.name);
                    this.NavigationService.Navigate(jbPage);
                }
            }
            else
            {
                MessageBox.Show("Selected Field is Empty", Title = "Empty Field Selected");
            }
        }
    }
}
