using GumusFit.Application.DTOs;

namespace GumusFit.Application.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllUsersAsync();
}
