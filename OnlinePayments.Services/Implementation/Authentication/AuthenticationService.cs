using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OnlinePayments.Repositories.Helpers;
using OnlinePayments.Repositories.Interfaces;
using OnlinePayments.Services.DTOs.Authentication;
using OnlinePayments.Services.Helpers;
using OnlinePayments.Services.Interfaces.Authentication;

namespace OnlinePayments.Services.Implementation.Authentication
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository employeeRepository)
        {
            _userRepository = employeeRepository;
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Username and password are required"
                };
            }

            var hashedPassword = AuthenticationHelper.HashPassword(request.Password);
            var filter = new Filter();
            filter.AddCondition("Username", request.Username);

            var users = await _userRepository.RetrieveCollection(filter);
            var user = users.FirstOrDefault();

            if (user == null || user.Password != hashedPassword)
            {
                return new LoginResponse
                {
                    Success = false,
                    Error = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                Success = true,
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName
            };
        }
    }
}
