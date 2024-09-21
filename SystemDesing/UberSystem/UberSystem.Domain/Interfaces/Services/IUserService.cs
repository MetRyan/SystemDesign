using UberSystem.Domain.Entities;

namespace UberSystem.Domain.Interfaces.Services
{
    public interface IUserService
	{
        Task<User> FindByEmail(string  email);


        Task Update(User user);
        Task  Add(User user);
        Task<User> getUserbyId(long id);

        Task<IEnumerable<User>> getAllUserCustomer();
        Task<IEnumerable<User>> getAllUserdriver();

        Task<bool> Login(User user);
        Task CheckPasswordAsync(User user);

        Task<IEnumerable<User>> GetCustomers();
        Task<bool> VerifyEmail(string token);

         Task<bool> ValidateVerificationToken(string token);
        Task SendEmailCustomer(string email);
        Task SendVerificationEmail(string email, User user);
          Task Delete(long userid);
        Task<User?> Login(string email, string password);
        Task<User> DecodeVerificationToken(string token);
    }
}

