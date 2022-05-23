using DockerApps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerApps.Interfaces
{
    public interface IProductLogic
    {
        Task<IEnumerable<Product>> GetProductsForCategory(string category);
    }
}
