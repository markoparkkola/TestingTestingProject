using Marko.Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marko.Services
{
    public interface ICarSalesman
    {
        Task<IEnumerable<Car>> GetCars();
        Task<(byte[], string)> GetDefaultImage(int carid);
        Task<IEnumerable<int>> GetImages(int carid);
        Task<(byte[], string)> GetImage(int pictureid);
    }
}
