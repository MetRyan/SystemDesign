using Microsoft.EntityFrameworkCore;
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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> addRating(Rating rating)
        {
            var getTrip = await _unitOfWork.Repository<Trip>().GetAsync(p => p.Id == rating.Id && p.Status == "3");
            if (getTrip == null) {
                throw new Exception(" The Trip is not Found  or not Done yet ");
            }
            var getDriver = await _unitOfWork.Repository<Driver>().GetAsync(p => p.UserId == rating.DriverId);
            if(getDriver == null)
            {
                throw new Exception(" The Driver not Found");
            }
            var newRating = new Rating
            {
                CustomerId = rating.CustomerId,
                DriverId = rating.DriverId,
                TripId = rating.TripId,
                Feedback = rating.Feedback,
                Rating1 = rating.Rating1,
            };

            try
            {
                var ratingRepository = _unitOfWork.Repository<Rating>();
                await _unitOfWork.BeginTransaction();
                // Use InsertAsync instead of InsertRangeAsync
                await ratingRepository.InsertAsync(newRating);
                // Commit the transaction after successful insert
                await _unitOfWork.CommitTransaction();

                return true; // Indicate success
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency exception (optional)
                // Log the exception or throw it again
                await _unitOfWork.RollbackTransaction();
                throw new Exception("Concurrency error occurred.", ex);
            }


        }

        public async Task<IEnumerable<Rating>> getAll()
        {
            return await _unitOfWork.Repository<Rating>().GetAllAsync();   
        }
    }
}
