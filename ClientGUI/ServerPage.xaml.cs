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
            while (true)
            {
                Task<List<Clients>> task = new Task<List<Clients>>(updateGUIClients);

                task.Start();
            if (task.Wait(TimeSpan.FromSeconds(30)))
            {
                List<Clients> clients = await task;

                //display clients
                Clients clis;
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
                await Task.Delay(TimeSpan.FromSeconds(10));
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
