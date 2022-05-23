using DockerApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerApps.Integrations
{
    public interface IOrderProcessingNotification
    {
        void QuickOrderReceived(QuickOrder order, int customerId, Guid orderId);
    }
}
