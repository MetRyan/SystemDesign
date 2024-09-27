using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
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
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDriverService _driverService;
        private readonly IConfiguration _configuration;


        public TripService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IDriverService _driverService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _driverService = _driverService;
        }

        public async Task driverConfirmOrder(long tripId, long driverId)
        {

            try { 
            
           var tripRepository = _unitOfWork.Repository<Trip>();
                await _unitOfWork.BeginTransaction();

                var getTrip = await getTripById(tripId);
                getTrip.Status = "1";
                getTrip.DriverId = driverId;

               await tripRepository.UpdateAsync(getTrip);   

               await _unitOfWork.CommitTransaction();


            }catch(DbUpdateException) { 
                    _unitOfWork.RollbackTransaction();
                throw;
            
            }
        }

        public async Task<IEnumerable<Trip>> getAllTripNoDriverYet()
        {
            try {
               var tripRepository=  _unitOfWork.Repository<Trip>();
                var getAll = await tripRepository.GetAllAsync();
                var fillterlist = getAll.Where(p => p.Status == "0" );
                    return fillterlist;

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);            
            }

        }

        public async Task<IEnumerable<Trip>> getAllTripNoDriverYet(long driverid)
        {
            try
            {
                var tripRepository = _unitOfWork.Repository<Trip>();
                var getAllTrips = await tripRepository.GetAllAsync();
                var getDriver = await _driverService.GetDriverbyId(driverid);
                if (getDriver == null)
                {
                    throw new Exception("Invalid Driver");

                }


                // Filter trips where status is "0" and within 2km from driver's location
                var filteredList = getAllTrips
                    .Where(trip => trip.Status == "0" && CalculateDistance((double)getDriver.LocationLatitude,(double)getDriver.LocationLongitude, (double)trip.SourceLatitude,(double) trip.SourceLongitude) <= 2.0)
                    .ToList();

                return filteredList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of Earth in km
            var latDistance = DegreeToRadian(lat2 - lat1);
            var lonDistance = DegreeToRadian(lon2 - lon1);
            var a = Math.Sin(latDistance / 2) * Math.Sin(latDistance / 2)
                    + Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2))
                    * Math.Sin(lonDistance / 2) * Math.Sin(lonDistance / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Distance in km
            return distance;
        }

        private double DegreeToRadian(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public async Task<Trip> getTripById(long id)
        {
            try {
                return await  _unitOfWork.Repository<Trip>().FindAsync(id);
            
            
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);    
            }
        }

        public async Task<long> tripOrder(Trip trip)
        {
            try { 
            var tripRepository = _unitOfWork.Repository<Trip>();

            await _unitOfWork.BeginTransaction();
            await tripRepository.InsertAsync(trip);
            await _unitOfWork.CommitTransaction();
             }
            catch (DbUpdateException dbEx)
            {
                await _unitOfWork.RollbackTransaction();
                // Log inner exception for details
                Console.WriteLine(dbEx.InnerException?.Message);
                throw;
            }
            return trip.Id;

        }

        public async Task<bool> CustomerOrderStatus(long customerid)
        {
            try {
                var tripRepository = _unitOfWork.Repository<Trip>();
                var checkUser = await tripRepository.GetAsync(p => p.CustomerId == customerid && p.Status =="0");
                // =0 la dang order
                if(checkUser != null)
                {
                    throw new Exception(" bạn dangd có đơn hàng please try again");

                }
                return true;


            } catch(Exception ex) { throw new Exception(ex.Message); }

        }

        /*   public Task<IEnumerable<Trip>> getAllTripNoDriverYet(long driverId)
           {
           }*/
    }
}
