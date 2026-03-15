using AutoMapper;
using GumusFit.Application.DTOs;
using GumusFit.Application.Services;
using GumusFit.Domain.Interfaces;

namespace GumusFit.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<UserDto>>(users);
    }
}
