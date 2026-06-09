using Orders.Api.Application.Dtos;
using Refit;

namespace Orders.Api.Interfaces
{
    public interface IUserClient
    {
        [Get("/api/users/{id}")]
        Task<UserDto> GetUserByIdAsync(Guid id);
    }
}
