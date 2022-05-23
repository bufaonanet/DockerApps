using DockerApps.Models;
using System;
using System.Threading.Tasks;

namespace DockerApps.Interfaces
{
    public interface IQuickOrderLogic
    {
        Task<Guid> PlaceQuickOrder(QuickOrder order, int customerId);
    }
}
