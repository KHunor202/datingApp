using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static async Task<UserDTO> ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email!,
            ImageUrl = user.ImageUrl,
            DisplayName = user.DisplayName,
            Token = await tokenService.CreateToken(user)
        };
    }
}
