using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Enums;
using UberSystem.Domain.Interfaces;
using UberSystem.Domain.Interfaces.Services;
using UberSytem.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace UberSystem.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<long, CancellationTokenSource> _cancellationTokens = new Dictionary<long, CancellationTokenSource>();

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public async Task Add(User user)
        {
            try
            {
                var userRepository = _unitOfWork.Repository<User>();
                var customerRepository = _unitOfWork.Repository<Customer>();
                var driverRepository = _unitOfWork.Repository<Driver>();
                if (user is not null)
                {
                    await _unitOfWork.BeginTransaction();
                    // check duplicate user
                    var existedUser = await userRepository.GetAsync(u => u.Id == user.Id && u.Email == user.Email);
                    
                    if (existedUser is not null) throw new Exception("User already exists.");


                        await userRepository.InsertAsync(user);

                    // add customer or driver into tables
                    if (user.Role == (int)UserRole.CUSTOMER)
                    {
                        var customer = _mapper.Map<Customer>(user);
                        await customerRepository.InsertAsync(customer);
                    }
                    else if (user.Role == (int)UserRole.DRIVER)
                    {
                        var driver = _mapper.Map<Driver>(user);
                        await driverRepository.InsertAsync(driver);
                    }


                          await _unitOfWork.CommitTransaction();
                 //   await SendVerificationEmail(user.Email, user.Id);
                    // long id = 10;
                  //  await Delete(id);
              //   ScheduleUserDeletionIfNotVerified(user.Id, TimeSpan.FromSeconds(20));
                   // send doi nguoi dung xac thuc xong bam vao duong link nếu endpoint getVerify được gọi thì mới commitTransaction thì sao như thế nào 

                    //   await SendEmailCustomer(user.Email);

                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

    /*    private void ScheduleUserDeletionIfNotVerified(long id, TimeSpan expirationTime)
        {
            var cts = new CancellationTokenSource();
            _cancellationTokens[id] = cts; // Store the CancellationTokenSource for the user

            Task.Run(async () =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await unitOfWork.BeginTransaction();

                    try
                    {
                        var userRepository = unitOfWork.Repository<User>();
                        var user = await userRepository.GetAsync(u => u.Id == id);
                        if (user != null) // Kiểm tra trạng thái xác thực
                        {
                            await Delete(id); // Xóa người dùng nếu chưa xác thực
                        }
                        await unitOfWork.CommitTransaction();
                    }
                    catch (TaskCanceledException)
                    {
                        // Task was cancelled due to successful verification
                        Console.WriteLine($"User {id} verification completed, deletion task cancelled.");
                    }
                    finally
                    {
                        _cancellationTokens.Remove(id); // Cleanup the token
                    }
                }
            }
            );
        }*/

        public async Task<bool> VerifyEmail(string token)
        {
            var userId = await ValidateVerificationToken(token);
            if (userId == null)
            {
                throw new Exception("Invalid token.");
            }

            var userRepository = _unitOfWork.Repository<User>();
            var user =  userRepository.FindAsync(userId);

            if (user == null)
            {
               throw new Exception ("User not found.");
            }

            // Kiểm tra xem email đã được xác thực chưa
            /*if (_isEmailVerified)
            {
                throw new Exception("Email already verified.");
            }
*/
  /*          user.IsVerified = true; */// Cập nhật trạng thái xác thực
           // await Update(user);
            await _unitOfWork.CommitTransaction(); // Commit transaction khi xác thực thành công

            // _isEmailVerified = true; // Đánh dấu rằng email đã được xác thực
            return true;

        }
    

    public async Task SendVerificationEmail(string email, User user)
        {
            var token = GenerateVerificationToken(user);

            var verificationLink = $"https://localhost:7094/api/uber-system/verify-email?token={token}"; // Thay đổi URL cho phù hợp

            var subject = "Email Verification";
            var body = $"<p>Please verify your email by clicking the link below:</p><p><a href='{verificationLink}'>Verify Email</a></p>";

            // Gửi email
            await _emailService.SendEmailAsync(email, subject, body);

        }
        private string GenerateVerificationToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()),
                    new Claim("role", user.Role.ToString()),
                    new Claim("userName", user.UserName.ToString()),
                    new Claim("email", user.Email.ToString()),
                    new Claim("password", user.Password.ToString()),


                }),
                Expires = DateTime.UtcNow.AddSeconds(600),  
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //check token 
        public async Task  <bool> ValidateVerificationToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

          /*  foreach (var cts in _cancellationTokens.Values)
            {
                cts.Cancel(); // Cancel the deletion task
            }*/
            try
            {
                if(token!= null) { 
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Ngăn việc chấp nhận token sau khi hết hạn
                }, out SecurityToken validatedToken);

                }
                else {
                    long id = 10;
                    await Delete(10);

                }
                return true;
                


            }
            catch
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
                //check khong con token

                if (userIdClaim != null)
                {
                    long userId = long.Parse(userIdClaim.Value);
                    // Find and delete the user
                    await Delete(userId);
                }
                return false;
            }
        }

        public Task CheckPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _unitOfWork.Repository<User>().FindAsync(email);
        }

        public async Task<IEnumerable<User>> GetCustomers()
        {
            var userRepository = _unitOfWork.Repository<User>();
            var users = await userRepository.GetAllAsync();

            var customers = users.Where(u => u.Role == (int)UserRole.CUSTOMER);
            return customers;
        }

        public async Task<bool> Login(User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var UserRepos = _unitOfWork.Repository<User>();
                var objUser = await UserRepos.FindAsync(user.Email);
                if (objUser == null)
                    return false;
                if (objUser.Password != user.Password)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> Login(string email, string password)
        {
            var userRepository = _unitOfWork.Repository<User>();
            var users = await userRepository.GetAllAsync();

            var user = users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user;
        }

        public async Task SendEmailCustomer(string email)
        {
            var getcustomer = await FindByEmail(email);

            var subject = "Uber - Registration completed";
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<style>");
            sb.Append("body { font-family: Arial, sans-serif; line-height: 1.6; }");
            sb.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            sb.Append("th, td { padding: 8px; border: 1px solid #dddddd; text-align: center; }");
            sb.Append("th { background-color: #5e8583; color: white; }");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append($"<p>Dear {getcustomer.UserName},</p>");
            sb.Append("<p>Congratulations! Your Uber account has been created successfully, and we are pleased to welcome you to our community.</p>");
            sb.Append("<p>We recommend you keep this email to store your credentials.</p>");
            sb.Append($"<p>Your credentials:<br/> E-mail address: {getcustomer.Email}</p>");
            sb.Append("<p>Best regards,<br/>");
            sb.Append("Uber</p>"); // Uncomment this line if you want to include the signature
            sb.Append("</body>");
            sb.Append("</html>");


            var emailBody = sb.ToString();
            try
            {
                await _emailService.SendEmailAsync(getcustomer.Email, subject, emailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task Update(User user)
        {
            try
            {
                var userRepository = _unitOfWork.Repository<User>();
                if (user is not null)
                {
                    await _unitOfWork.BeginTransaction();
                    await userRepository.UpdateAsync(user);
                    await _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }
        public async Task Delete(long userid)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var userRepository =  _unitOfWork.Repository<User>();
                var cusRepository = _unitOfWork.Repository<Customer>();
                var DriRepository = _unitOfWork.Repository<Driver>();

                var user = await userRepository.GetAsync(p => p.Id == userid);
                var cus = await cusRepository.GetAsync(p => p.Id == userid);
                var dri = await DriRepository.GetAsync(p => p.Id == userid);

                if (user is not null)
                {
                    await _unitOfWork.BeginTransaction();

                    if (cus is not null)
                    {
                        await cusRepository.DeleteAsync(cus);

                    }
                    else if (dri is not null) {
                        await DriRepository.DeleteAsync(dri);

                    }

                    await userRepository.DeleteAsync(user);

                    await _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<User> getUserbyId(long id)
        {
            return await _unitOfWork.Repository<User>().GetAsync(p => p.Id == id);

        }

        public async Task<IEnumerable< User>> getAllUserCustomer()
        {
            var listUser = await _unitOfWork.Repository<User>().GetAllAsync();
            return listUser.Where(p => p.Role == 0);
        }

        public async Task<IEnumerable< User>> getAllUserdriver()
        {
            var listUser = await _unitOfWork.Repository<User>().GetAllAsync();
            return listUser.Where(p => p.Role == 1);
        }

        public async Task<User> DecodeVerificationToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Lấy thông tin từ JWT
                var userId = jwtToken.Claims.First(c => c.Type == "id").Value;
              var roleclaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                var email = jwtToken.Claims.First(c => c.Type == "email")?.Value;
                var userName = jwtToken.Claims.First(c => c.Type == "userName")?.Value;
                var password = jwtToken.Claims.First(c => c.Type == "password")?.Value;
                // Tạo đối tượng User từ thông tin đã giải mã
                var user = new User
                {
                    Id = long.Parse(userId),
                   Role = int.Parse(roleclaim),
                    Email = email,
                    UserName = userName,
                    Password = password // Nên mã hóa lại trước khi lưu vào cơ sở dữ liệu
                };

                return user;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }

        }
    }
}

