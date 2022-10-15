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
        Jobs downloadJobs(int id);

        [OperationContract]
        List<Jobs> connectServer(int id);

        [OperationContract]
        bool FinishingJob(int id, JobPool jp);
    }
}
