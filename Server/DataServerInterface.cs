using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface DataServerInterface
    {
        [OperationContract]
        List<Clients> GetClientsRegistered();

        [OperationContract]
        bool FinishingJob(Jobs job);
    }
}
