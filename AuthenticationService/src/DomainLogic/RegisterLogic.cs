//using src.DataTransferObject.Parameter;
//using src.DataTransferObject.ResultData;
//using src.Infrastructure.EF.Models;
//using src.Infrastructure.Repositories.Interfaces;
//using System.Security.Cryptography;
//using System.Text;
//using Shared.Contracts.Interfaces;
//using Shared.Contracts.Enums;

//namespace src.DomainLogic
//{
//    public class RegisterLogic : IDomainLogic<RegisterParam, Task<RegisterResultData>>
//    {
//        private readonly IUserRepository _userRepository;

//        public RegisterLogic(IUserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        public async Task<RegisterResultData> Execute(RegisterParam param)
//        {
//            if (param == null)
//            {
//                throw new ArgumentNullException(nameof(param));
//            }

//            var existed = await _userRepository.GetUserByEmail(param.Email);

//            if (existed != null) {
//                return new RegisterResultData
//                {
//                    Result = false,
//                    Message = "Email is registerted",
//                    StatusCode = StatusCodeEnum.BadRequest,
//                };
//            }

//            var newUser = new User
//            {
//                Id = Guid.NewGuid(),
//                Email = param.Email,
//                Password = HashPassword(param.Password),
//            };

//            await _userRepository.CreateUser(newUser);


//        }

//        private string HashPassword(string password)
//        {
//            using (var sha256 = SHA256.Create())
//            {
//                var bytes = Encoding.UTF8.GetBytes(password);
//                var hash = sha256.ComputeHash(bytes);
//                var hashedPassword = Convert.ToBase64String(hash);

//                return hashedPassword;
//            }
//        }

//    }
//}
