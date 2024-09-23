﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;

namespace UberSystem.Domain.Interfaces.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<Driver>> getAvailableDrivers(double pickupLatitude, double pickupLongitude);

        Task<Driver> GetDriverbyId(long id);    

/*        Task<IEnumerable<Driver>> getAllDriver();
*/
    }
}