using Grpc.Core;
using ProfileGrpc;
using ProfileService.DataTransferObject.Parameter;
using ProfileService.DomainLogic;
using ProfileService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Exceptions;
using AutoMapper;

namespace ProfileService.Services
{
    public class ProfileGrpcServiceImpl : ProfileGrpcService.ProfileGrpcServiceBase
    {
        private readonly GetProfileLogic _getProfileLogic;
        private readonly CreateProfileLogic _createProfileLogic;
        private readonly IMapper _mapper;
        private readonly IStaffRepository _staffRepository;
        private readonly ICustomerRepository _customerRepository;

        public ProfileGrpcServiceImpl(CreateProfileLogic createProfileLogic, 
                                      IStaffRepository staffRepository, 
                                      ICustomerRepository customerRepository, 
                                      GetProfileLogic getProfileLogic, 
                                      IMapper mapper)
        {
            _createProfileLogic = createProfileLogic;
            _staffRepository = staffRepository;
            _customerRepository = customerRepository;
            _getProfileLogic = getProfileLogic;
            _mapper = mapper;
        }

        public override async Task<CreateProfileGrpcReplyDTO> CreateProfile(CreateProfileGrpcRequestDTO request, ServerCallContext context)
        {
            DateTime? dob = null;
            if (!string.IsNullOrEmpty(request.DayOfBirth) &&
                DateTime.TryParse(request.DayOfBirth, out var parsed))
            {
                dob = DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
            }

            Guid? cinemaId = null;
            if (!string.IsNullOrEmpty(request.CinemaId))
            {
                cinemaId = Guid.Parse(request.CinemaId);
            }

            var result = await _createProfileLogic.Execute(new CreateProfileParam
            {
                UserId = Guid.Parse(request.UserId),
                FullName = request.FullName,
                Role = request.Role,
                Salary = decimal.TryParse(request.Salary, out var sal) ? sal : 0,
                PhoneNumber = request.PhoneNumber,
                DayOfBirth = dob,
                Gender = request.Gender,
                CinemaId = cinemaId,
                Position = request.Position,
            });

            return new CreateProfileGrpcReplyDTO
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = (int)result.StatusCode
            };
        }

        public override async Task<GetStaffByUserIdReply> GetStaffByUserId(
            GetStaffByUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return new GetStaffByUserIdReply
                {
                    Found = false
                };
            }

            var staff = await _staffRepository.GetStaffByUserId(userId);

            if (staff == null)
            {
                return new GetStaffByUserIdReply
                {
                    Found = false
                };
            }

            return new GetStaffByUserIdReply
            {
                Found = true,
                UserId = staff.UserId.ToString(),
                FullName = staff.FullName,
                PhoneNumber = staff.PhoneNumber ?? string.Empty,
                DayOfBirth = staff.DayOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                Gender = staff.Gender ?? string.Empty,
                CinemaId = staff.CinemaId.ToString(),
                Position = staff.Position,
                Salary = staff.Salary.ToString()
            };
        }  

        public override async Task<DeleteProfileReply> DeleteCustomerByUserId(
            DeleteProfileByUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return new DeleteProfileReply
                {
                    Result = false,
                    Message = "Invalid user id",
                    StatusCode = 404
                };
            }

            await _customerRepository.SoftDeleteByUserId(userId);

            return new DeleteProfileReply
            {
                Result = true,
                Message = "Customer profile deleted",
                StatusCode = 200
            };
        }

    public override async Task<DeleteProfileReply> DeleteStaffByUserId(
        DeleteProfileByUserIdRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return new DeleteProfileReply
            {
                Result = false,
                Message = "Invalid user id",
                StatusCode = 400
            };
        }

        await _staffRepository.SoftDeleteByUserId(userId);

        return new DeleteProfileReply
        {
            Result = true,
            Message = "Staff profile deleted",
            StatusCode = 200
        };
    }

        public override async Task<GetCustomerByUserIdReply> GetCustomerByUserId(
            GetCustomerByUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                return new GetCustomerByUserIdReply
                {
                    Found = false
                };
            }

            var customer = await _customerRepository.GetCustomerByUserId(userId);

            if (customer == null)
            {
                return new GetCustomerByUserIdReply
                {
                    Found = false
                };
            }

            return new GetCustomerByUserIdReply
            {
                Found = true,
                UserId = customer.UserId.ToString(),
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumer ?? string.Empty,
                DayOfBirth = customer.DayOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                Gender = customer.Gender ?? string.Empty,
                Points = customer.Points
            };
        }

        public override async Task<UpdateProfileGrpcReplyDTO> UpdateCustomerByUserId(
            UpdateCustomerByUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                throw new ValidationException("Invalid user id");
            }

            var customer = await _customerRepository.GetCustomerByUserId(userId);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))
                customer.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                customer.PhoneNumer = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.DayOfBirth))
            {
                if (DateOnly.TryParse(request.DayOfBirth, out var dob))
                    customer.DayOfBirth = dob;
            }

            if (!string.IsNullOrWhiteSpace(request.Gender))
                customer.Gender = request.Gender;

            if (!string.IsNullOrWhiteSpace(request.Points) &&
                int.TryParse(request.Points, out var pts))
            {
                customer.Points = pts;
            }

            await _customerRepository.UpdateCustomer(customer);

            return new UpdateProfileGrpcReplyDTO
            {
                Result = true,
                Message = "Customer updated successfully",
                StatusCode = 200
            };
        }

        public override async Task<UpdateProfileGrpcReplyDTO> UpdateStaffByUserId(
            UpdateStaffByUserIdRequest request,
            ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                throw new ValidationException("Invalid user id");
            }

            var staff = await _staffRepository.GetStaffByUserId(userId);
            if (staff == null)
            {
                throw new NotFoundException("Staff not found");
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))
                staff.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                staff.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.DayOfBirth) &&
                DateTime.TryParse(request.DayOfBirth, out var dob))
            {
                staff.DayOfBirth = DateTime.SpecifyKind(dob, DateTimeKind.Utc);
            }

            if (!string.IsNullOrWhiteSpace(request.Gender))
                staff.Gender = request.Gender;

            if (!string.IsNullOrWhiteSpace(request.CinemaId) &&
                Guid.TryParse(request.CinemaId, out var cinemaId))
            {
                staff.CinemaId = cinemaId;
            }

            if (!string.IsNullOrWhiteSpace(request.Position))
                staff.Position = request.Position;

            if (!string.IsNullOrWhiteSpace(request.Salary) &&
                decimal.TryParse(request.Salary, out var salary))
            {
                staff.Salary = salary;
            }

            await _staffRepository.UpdateStaff(staff);

            return new UpdateProfileGrpcReplyDTO
            {
                Result = true,
                Message = "Staff updated successfully",
                StatusCode = 200
            };
        }

        public override async Task<GetStaffByUserIdReply> GetStaffs(
            GetStaffsRequest request,
            ServerCallContext context)
        {
            Guid? userId = null;
            Guid? cinemaId = null;

            if (!string.IsNullOrWhiteSpace(request.UserId) &&
                Guid.TryParse(request.UserId, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            if (!string.IsNullOrWhiteSpace(request.CinemaId) &&
                Guid.TryParse(request.CinemaId, out var parsedCinemaId))
            {
                cinemaId = parsedCinemaId;
            }

            var staff = await _staffRepository.GetStaffAsync(userId, cinemaId);

            if (staff == null)
            {
                return new GetStaffByUserIdReply
                {
                    Found = false
                };
            }

            return new GetStaffByUserIdReply
            {
                Found       = true,
                UserId      = staff.UserId.ToString(),
                FullName    = staff.FullName ?? string.Empty,
                PhoneNumber = staff.PhoneNumber ?? string.Empty,
                DayOfBirth  = staff.DayOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty,
                Gender      = staff.Gender ?? string.Empty,
                CinemaId    = staff.CinemaId.ToString(),
                Position    = staff.Position ?? string.Empty,
                Salary      = staff.Salary.ToString()
            };
        }

        public override async Task<GetProfileGrpcReplyDTO> GetProfile(GetProfileGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getProfileLogic.Execute(new GetProfileParam
            {
                UserId = Guid.Parse(request.UserId),
            });

            return _mapper.Map<GetProfileGrpcReplyDTO>(result);
        }
    }
}
