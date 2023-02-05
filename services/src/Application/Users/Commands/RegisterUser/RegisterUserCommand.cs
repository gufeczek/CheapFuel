using Application.Models;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Username, string Email, string Password, string ConfirmPassword) 
    : IRequest<UserDetailsDto>;