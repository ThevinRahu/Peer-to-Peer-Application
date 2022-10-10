using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Start the server
            Console.WriteLine("Welcome to server");
            //This represents a tcp / ip binding in the Windows network stack
            var tcp = new NetTcpBinding();
            //This is the actual host service system
            //Bind server to the implementation of DataServer
            var host = new ServiceHost(typeof(DataServer));
            //Present the publicly accessible interface to the client. 0.0.0.0 tells.net to accept on any interface. :8100 means this will use port 8100. DataService is a name for the actual service, this can be any string.
            host.AddServiceEndpoint(typeof(DataServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");
            //And open the host for business!
            host.Open();
            //Hold the server open until someone does something
            Console.WriteLine("System Online");
            Console.ReadLine();
            //Close the host
            host.Close();
        }
    }
}
