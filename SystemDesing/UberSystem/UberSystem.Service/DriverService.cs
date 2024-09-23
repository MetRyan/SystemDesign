using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces;
using UberSystem.Domain.Interfaces.Services;

namespace UberSystem.Service
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public DriverService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

       /* public Task<IEnumerable<Driver>> getAllDriver()
        {
            throw new NotImplementedException();
        }*/

        public async Task<IEnumerable<Driver>> getAvailableDrivers(double pickupLatitude, double pickupLongitude)
        {
            var driverRepository =  _unitOfWork.Repository<Driver>();   

            var drivers = await driverRepository.GetAllAsync();

            // Tính khoảng cách giữa điểm đón và tài xế, lọc trong bán kính 2km
            var availableDrivers = drivers
                .Where(driver => CalculateDistance(pickupLatitude, pickupLongitude,(double) driver.LocationLatitude, (double)driver.LocationLongitude) <= 2.0)
            //    .OrderByDescending(driver => driver.Ratings) // Sắp xếp theo rating
                .ToList();

            return availableDrivers;
        }

        public async Task<Driver> GetDriverbyId(long id)
        {
            return await _unitOfWork.Repository<Driver>().GetAsync(p => p.Id ==id);  
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Bán kính Trái đất (km)
            var latDistance = DegreeToRadian(lat2 - lat1);
            var lonDistance = DegreeToRadian(lon2 - lon1);
            var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                    + Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2))
                    * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Khoảng cách theo km
            return distance;
        }

        private double DegreeToRadian(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
