using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using src.ServiceConnector.ProfileServiceConnector;

namespace src.DomainLogic
{
    public class RegisterLogic : IDomainLogic<RegisterParam, Task<RegisterResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;
        public RegisterLogic(IUserRepository userRepository, ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<RegisterResultData> Execute(RegisterParam param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            var existed = await _userRepository.GetUserByEmail(param.Email);

            if (existed != null)
            {
                throw new ValidationException("Email is registerted");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = param.Email,
                Password = HashPassword(param.Password),
            };

            await _userRepository.CreateUser(newUser);

            var profile = await _profileServiceConnector.CreateProfile(param.FullName, newUser.Role, newUser.Id.ToString());

            if (!profile.Result)
            {
                await _userRepository.RemoveUser(newUser);
            }
            
            return new RegisterResultData
            {
                Result = true,
                Message = "Register Successfully",
                StatusCode = StatusCodeEnum.Success,
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var hashedPassword = Convert.ToBase64String(hash);

                return hashedPassword;
            }
        }

    }
}
