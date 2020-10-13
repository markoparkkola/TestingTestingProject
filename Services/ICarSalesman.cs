using Marko.Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marko.Services
{
    /// <summary>
    /// Interface to do all the car related stuff like loading them and their images.
    /// </summary>
    public interface ICarSalesman
    {
        /// <summary>
        /// Returns list of cars in sale.
        /// </summary>
        /// <returns>The list.</returns>
        Task<IEnumerable<Car>> GetCars();
        /// <summary>
        /// Return default image for the car.
        /// </summary>
        /// <param name="carid">Car id. This is ugly. One could use a key here which is not the same as database id.</param>
        /// <returns>Image in byte array and content type.</returns>
        Task<(byte[], string)> GetDefaultImage(int carid);
        /// <summary>
        /// Return list of all the images related to single car.
        /// </summary>
        /// <param name="carid">Car id.</param>
        /// <returns>List of image ids.</returns>
        Task<IEnumerable<int>> GetImages(int carid);
        /// <summary>
        /// Returns image for given image id.
        /// </summary>
        /// <param name="pictureid">Image id.</param>
        /// <returns>Image in byte array and content type.</returns>
        Task<(byte[], string)> GetImage(int pictureid);
    }
}
