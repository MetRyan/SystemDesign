using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;

namespace UberSystem.Domain.Interfaces.Services
{
    public interface ITripService
    {
        Task<long> tripOrder(Trip trip);
        // get all triporderStatus chua confirm
        Task<IEnumerable<Trip>> getAllTripNoDriverYet();
        //get tat ca tai xe dua tren km va rating 

        Task<IEnumerable<Trip>> getAllTripNoDriverYet(long driverId);

        //for user to view driver nearby 2km 




        //check xem User co dat chua mỗi user chỉ được dặt 1 lần;




        Task<bool> CustomerOrderStatus(long customerid);
        Task<IEnumerable< Trip>> getAllHistoryTripBaseDriverId(long driverId);



        //get Trip by id ne

        Task<Trip> getTripById(long id);
        // click confirm order

        Task <bool> driverConfirmOrder(long tripId, long driverId);

        Task<Trip>  getCurrentTripOfDriver(long driverId);

        Task<bool> DriverConfirmOrderDone(long tripId);


    }
}
