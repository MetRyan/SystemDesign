using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Enums;
using UberSystem.Domain.Interfaces.Services;
using UberSystem.Service;
using UberSytem.Dto;
using UberSytem.Dto.Requests;
using UberSytem.Dto.Responses;

namespace UberSystem.Api.Authentication.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, TokenService tokenService, IMapper mapper, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="request"></param>
        /// 
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponseModel<UserResponseModel>>> Login([FromBody] LoginModel request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _userService.Login(request.Email, request.Password);
            if (result is null) return NotFound(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "User not found"
            });
            var accessToken = _tokenService.GenerateToken(result, new List<string> { result.Role.ToString() });
            var response = _mapper.Map<UserResponseModel>(result);
            response.AccessToken = accessToken;

            return Ok(new ApiResponseModel<UserResponseModel>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success",
                Data = response
            });
        }

        /// <summary>
        /// Sign up into Uber System
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponseModel<string>>> Signup([FromBody] SignupModel request)
        {
            if (!ModelState.IsValid) return BadRequest();
            // Authenticate for role
            if (request.Role != (int)UserRole.CUSTOMER && request.Role != (int)UserRole.DRIVER && request.Role != (int)UserRole.ADMIN)
                return BadRequest(new ApiResponseModel<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid role's value in the system!"
                });
            var user = _mapper.Map<User>(request);

            //check email -> 

            await _userService.SendVerificationEmail(user.Email, user);



            return Ok(new ApiResponseModel<string>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Success",
            });
        }
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            //var userId = await _userService.ValidateVerificationToken(token);

            var user = _userService.DecodeVerificationToken(token);

            if (user != null)
            {
                await _userService.Add(await user);

                // Xử lý thêm thông tin người dùng nếu cần
                return Ok("Email verified successfully.");
            }
            return BadRequest("Invalid token.");
        }

        // [HttpPost("sign-up")]
        // [AllowAnonymous]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<ActionResult<ApiResponseModel<string>>> Signup([FromBody] SignupModel request)
        // {
        //     if (!ModelState.IsValid) return BadRequest();
        //     // Authenticate for role
        //     if (request.Role != (int)UserRole.CUSTOMER && request.Role != (int)UserRole.DRIVER && request.Role != (int)UserRole.ADMIN)
        //         return BadRequest(new ApiResponseModel<string>
        //         {
        //             StatusCode = HttpStatusCode.BadRequest,
        //             Message = "Invalid role's value in the system!"
        //         });
        //     var user = _mapper.Map<User>(request);

        //     //check email -> 

        //     await _userService.SendVerificationEmail(user.Email, user);



        //     return Ok(new ApiResponseModel<string>
        //     {
        //         StatusCode = HttpStatusCode.OK,
        //         Message = "Success",
        //     });
        // }
        [HttpGet("get-all-user")]
        public async Task<ActionResult<IEnumerable<UserReponseInformation>>> getAll()
        {
            var getList =  await _userService.getAll();
            var UserReponse = _mapper.Map<IEnumerable<UserReponseInformation>>(getList);
            if (UserReponse == null)
            {
                return NotFound(new ApiResponseModel<IEnumerable<UserReponseInformation>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No users found."
                });
            }
            else
            {
                return Ok(new ApiResponseModel<IEnumerable<UserReponseInformation>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = UserReponse,

                });

          }


        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(long id = 10)
        {
            try
            {
                await _userService.Delete(id);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
