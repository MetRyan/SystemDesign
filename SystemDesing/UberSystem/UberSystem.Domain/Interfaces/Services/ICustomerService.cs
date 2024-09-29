using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberSystem.Domain.Entities;

namespace UberSystem.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Rating>> getAll();
        Task <bool> addRating(Rating rating);
    }
}
