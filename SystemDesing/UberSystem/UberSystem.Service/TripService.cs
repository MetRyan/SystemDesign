using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class TripService : ITripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public TripService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
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
    }
}
