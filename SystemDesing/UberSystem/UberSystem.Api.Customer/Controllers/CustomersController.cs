using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UberSystem.Domain.Interfaces.Services;
using UberSystem.Infrastructure;
using UberSystem.Service;
using UberSytem.Dto;
using UberSytem.Dto.Responses;

namespace UberSystem.Api.Customer.Controllers
{
    public class CustomersController : BaseApiController
    {

     
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public CustomersController(IUserService userService, TokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
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
            var UserReponse = _mapper.Map<IEnumerable< UserReponseInformation>>(listUser);

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

        /// <summary>
        /// Retrieve customers in system
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// </remarks>
        [HttpGet("customers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Domain.Entities.Customer>> GetCustomer(long id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomer(long id, Domain.Entities.Customer customer)
        //{
        //    if (id != customer.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

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
