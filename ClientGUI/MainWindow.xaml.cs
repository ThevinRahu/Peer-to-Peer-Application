using Newtonsoft.Json;
using RestSharp;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();

            Thread clock = new Thread(updateGUIClients);
            
        }

        public void updateGUIClients()
        {
            while (true)
            {
                //thread handling
                Thread.Sleep(2 * 60000);

                //display clients
                RestClient restClient = new RestClient("http://localhost:9987/");
                RestRequest restRequest = new RestRequest("api/clients/", Method.Get);
                RestResponse restResponse = restClient.Execute(restRequest);

                List<Clients> clients = JsonConvert.DeserializeObject<List<Clients>>(restResponse.Content);

                //display clients
            }
        }
    }
}
