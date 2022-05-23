using DockerApps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DockerApps.Repository
{
    public interface ICarvedRockRepository
    {
        Task<List<Product>> GetProducts(string category);
        Task SubmitNewQuickOrder(QuickOrder order, int customerId, Guid orderId);
    }
}
