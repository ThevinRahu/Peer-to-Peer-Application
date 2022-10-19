using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    internal class InterfaceChannel
    {
        //Connecting the .net remoting server

        private DataServerInterface interfaceChannel;
        public DataServerInterface generateChannel(string URL)
        {
            ChannelFactory<DataServerInterface> channelFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //string URL = "net.tcp://localhost:8100/DataService";
            channelFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            interfaceChannel = channelFactory.CreateChannel();
            return interfaceChannel;
        }

    }
}
