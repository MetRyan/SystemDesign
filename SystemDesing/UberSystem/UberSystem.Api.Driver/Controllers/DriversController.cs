using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces.Services;
using UberSytem.Dto.Requests;
using UberSytem.Dto.Responses;
using UberSytem.Dto;

namespace UberSystem.Api.Driver.Controllers
{
    public class DriversController : BaseApiController
    {
        /* private readonly ITripService _tripService;*/

        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITripService _tripService;
        private readonly IPaymentService _paymentService;


        public DriversController(IMapper mapper, IUserService userService, ITripService tripService, IPaymentService paymentService)
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
        [HttpGet("get-all-driver")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserReponseInformation>>> getDriver()
        {
                var listUser = await _userService.getAllUserdriver();
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

        [HttpGet("get-all-trip-no-Confirm-yet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TripReponse>>> GetAllTripNoConfirmyet()
        {
            var listRepository = await _tripService.getAllTripNoDriverYet();
            var listReponse = _mapper.Map<IEnumerable<TripReponse>>(listRepository);

            if (listReponse == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No Trip not found"
                });
            }
            return Ok(new ApiResponseModel<IEnumerable<TripReponse>>


            {
                StatusCode = HttpStatusCode.OK,
                Data = listReponse

            });
        }

        [HttpGet("get-all-trip-base-location-driver/{driverId}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TripReponse>>> GetAllTripbaseDrivernoConfirmyet(long driverId)
        {
            var listRepository = await _tripService.getAllTripNoDriverYet(driverId);
            var listReponse = _mapper.Map<IEnumerable<TripReponse>>(listRepository);

            if (listReponse == null)
            {
                return NotFound(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No Trip not found"
                });
            }
            return Ok(new ApiResponseModel<IEnumerable<TripReponse>>


            {
                StatusCode = HttpStatusCode.OK,
                Data = listReponse,
                Message = "Only list < 2km"


            });
        }


        /* [HttpGet("ListTrip")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<IEnumerable<UserReponseInformation>>> GetCustomers()
         {
             var listUser = await _userService.getAllUserdriver();
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
         }*/

        /*        [HttpPost]
                [ProducesResponseType(StatusCodes.Status200OK)]
                public async Task<ActionResult> CreateTrip(TripRequest request)
                {
                    try
                    {
                        var trip = _mapper.Map<Trip>(request);
                        trip.Status = "0";
                        var paymentId = await _tripService.tripOrder(trip);
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


                }*/

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
       /* [HttpPost]

        public async Task<IActionResult> ConfirmOrder(float tripid, float driverid)
        {
           

        }*/


        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(long id, User Driver)
        {
            if (id != Driver.Id)
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
            try
            {
                await _userService.Update(Driver);

            }
            catch (Exception ex)
            {
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
        public async Task<IActionResult> DeleteDriver(long id)
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
    }
}
