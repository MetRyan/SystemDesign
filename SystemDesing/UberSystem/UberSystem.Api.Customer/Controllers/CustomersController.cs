﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces.Services;
using UberSystem.Infrastructure;
using UberSystem.Service;
using UberSytem.Dto;
using UberSytem.Dto.Requests;
using UberSytem.Dto.Responses;

namespace UberSystem.Api.Customer.Controllers
{
    public class CustomersController : BaseApiController
    {

     
       /* private readonly ITripService _tripService;*/

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITripService _tripService;
        private readonly IPaymentService _paymentService;


        public CustomersController(IMapper mapper, IUserService userService, ITripService tripService, IPaymentService paymentService)
        {
            _mapper = mapper;
            _userService = userService;
            _tripService = tripService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Retrieve customers in system
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet("customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReponseInformation>>> GetCustomers()
        {
            var listUser = await _userService.getAllUserCustomer();
            var UserReponse = _mapper.Map<IEnumerable<UserReponseInformation>>(listUser);

            if (UserReponse == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found"
                });
            }
            return Ok(new ApiResponseModel<IEnumerable<UserReponseInformation>>


            {
                StatusCode = HttpStatusCode.OK,
                Data = UserReponse
                
            });
        }

        [HttpGet("driver-2km/{userid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReponseInformation>>> getDrivernearby(long userId)
        {
            var getUser = await _userService.getAllUserCustomer();
            var UserReponse = _mapper.Map<IEnumerable<UserReponseInformation>>(getUser);

            if (UserReponse == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found"
                });
            }
            return Ok(new ApiResponseModel<IEnumerable<UserReponseInformation>>


            {
                StatusCode = HttpStatusCode.OK,
                Data = UserReponse

            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateTrip(TripRequest request)
        {
            try
            {
                var trip = _mapper.Map<Trip>(request);
                trip.Status = "0";
                var paymentId= await _tripService.tripOrder(trip);
                var payment = _mapper.Map<Payment>(request);
                payment.TripId = paymentId;

                await _paymentService.AddPayment(payment);


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = " đã đặt đợi cho tài xế confirm"
            });


        }

        /// <summary>
        /// Retrieve customers in system
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// </remarks>
        /*[HttpGet("customers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserReponseInformation>> GetCustomer(long id)
        {
           
        }*/

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(long id, User customer)
            {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            var getUser = _userService.getUserbyId(id);
            if (getUser == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found"
                });
            }
            try { 
            await _userService.Update(customer);
            
            }catch (Exception ex) {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Somthing wrong with update please try again"
                });

            }
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Update successs"
            });


        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
          
            var getUser = _userService.getUserbyId(id);
            if (getUser == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "User not found"
                });
            }
            try
            {
                await _userService.Delete(id);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Somthing wrong with delete please try again"
                });

            }
            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "delete successs"
            });


        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Domain.Entities.Customer>> PostCustomer(Domain.Entities.Customer customer)
        //{
        //    if (_context.Customers == null)
        //    {
        //        return Problem("Entity set 'UberSystemDbContext.Customers'  is null.");
        //    }
        //    _context.Customers.Add(customer);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CustomerExists(customer.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        //}

        // DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCustomer(long id)
        //{
        //    if (_context.Customers == null)
        //    {
        //        return NotFound();
        //    }
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Customers.Remove(customer);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CustomerExists(long id)
        //{
        //    return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
